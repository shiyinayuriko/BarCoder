using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace BarCoder
{
    public class Data
    {
        private static Data _instance = null;
        public static Data Instance()
        {
            return _instance ?? (_instance = new Data());
        }

        private DataTable _dataTable = null;
        private StreamWriter _writer;

        public DataTable LoadExcel(string fileName)
        {
            FileStream file = null;
            DataTable table;
            try
            {
                file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                IWorkbook workbook;
                if (fileName.EndsWith(".xlsx")) workbook = new XSSFWorkbook(file);
                else workbook = new HSSFWorkbook(file);
                var sheet = workbook.GetSheetAt(0);

                table = new DataTable();
                
                var headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;
                int rowCount = sheet.LastRowNum;

                for (int i = 2; i < cellCount; i++)
                    table.ColNames.Add(headerRow.GetCell(i).StringCellValue);

                var names = table.ColNames;
                for (var r = 1; r <= rowCount; r++)
                {
                    var row = sheet.GetRow(r);
                    var id = (int) row.GetCell(0).NumericCellValue;
                    var code = row.GetCell(1).StringCellValue;
                    var item = new DataItem();
                    for (var i = 2; i < cellCount; i++)
                        item.Put(names[i - 2], row.GetCell(i) == null ? null : row.GetCell(i).ToString());

                    table.InserData(id,code,item);
                }
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                    file.Dispose();
                }
            }
            _dataTable = table;
            LoadData(fileName + ".log");
            InitialWriter(fileName + ".log");
            return table;
        }
        public DataTable LoadData(string fileName)
        {
            if (_instance == null) throw new Exception("请先导入excel数据库");
            var reader = new StreamReader(new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine()??"";
                var items = line.Split('\t')[0].Split(' ');
                var id = int.Parse(items[0]);
                var times = int.Parse(items[1]);
                if (!_dataTable.EnterInfo.ContainsKey(id))
                {
                    _dataTable.EnterInfo.Add(id, times + 1);
                }
                else
                {
                    _dataTable.EnterInfo[id] = times + 1;
                }

            }
            reader.Close();
            return _dataTable;
        }
        private void InitialWriter(string fileName)
        {
            _writer = new StreamWriter(new FileStream(fileName, FileMode.Append,FileAccess.Write));
        }
        
        public CheckResult Check(string code)
        {
            var id = _dataTable.GetId(code);
            if (id == -1)
            {
                WriteLog(code, id , -1);
                return new CheckResult(false, "错误编码", new Dictionary<string, string>(), _dataTable.Entered, _dataTable.Total);
            }

            var result = _dataTable.GetDataItem(id);

            if (_dataTable.EnterInfo.ContainsKey(id))
            {
                var times = _dataTable.EnterInfo[id];
                _dataTable.EnterInfo[id] = times + 1;

                WriteLog(code, id, times);
                return new CheckResult(false, "重复入场:" + times, result, _dataTable.Entered, _dataTable.Total);
            }
            else
            {
                _dataTable.EnterInfo.Add(id,1);
                WriteLog(code, id, 0);
                _dataTable.Entered ++;
                return new CheckResult(true, "通过", result, _dataTable.Entered, _dataTable.Total);
            }
        }

        public int GetId(string code)
        {
            return _dataTable.GetId(code);
        }

        private void WriteLog(string code,int id,int times)
        {
            var time = DateTime.Now.ToString(CultureInfo.InvariantCulture); ;
            _writer.WriteLine("{1} {2}\tcode:{0}\tid:{1}\ttimes:{2}\ttime:{3}", code, id, times, time);
            _writer.Flush();
        }
    }


    public class DataTable
    {
        public List<string> ColNames;
        private readonly Dictionary<int,DataItem> _data;
        private readonly Dictionary<string, int> _codeMap;
        public readonly Dictionary<int, int> EnterInfo;
        public int Total = 0;
        public int Entered = 0;
        public DataTable()
        {
            ColNames = new List<string>();
            _data = new Dictionary<int, DataItem>();
            _codeMap = new Dictionary<string, int>();
            EnterInfo = new Dictionary<int, int>();
        }
        public void InserData(int id ,string code, DataItem item)
        {
            _data.Add(id,item);
            _codeMap.Add(code, id);
            //EnterInfo.Add(id,0);
            Total = _data.Count;
        }

        public int GetId(string code)
        {
            if (_codeMap.ContainsKey(code))
            {
                return _codeMap[code];
            }
            return -1;
        }

        public Dictionary<string, string> GetDataItem(int id)
        {
            if (_data.ContainsKey(id))
            {
                return _data[id].GetAll();
            }
            return null;
        }
    }
    public class DataItem
    {
        private readonly Dictionary<string,string> _data;

        public DataItem()
        {
            _data = new Dictionary<string, string>();
        }
        public void Put(string key,string value)
        {
            _data.Add(key,value);
        }
        public string Get(string key)
        {
            return _data.ContainsKey(key) ? _data[key] : null;
        }
        public Dictionary<string, string> GetAll()
        {
            return _data;
        }
    }

    public class CheckResult
    {
        public bool IsSuccess;
        public string ErrorMessage;
        public Dictionary<string, string> UserInfo;
        public int Entered;
        public int Total;
        public CheckResult(bool isSuccess, string errormessage, Dictionary<string, string> userInfo,int entered,int total)
        {
            this.IsSuccess = isSuccess;
            this.ErrorMessage = errormessage;
            this.UserInfo = userInfo;
            this.Entered = entered;
            this.Total = total;
        }
    }
}

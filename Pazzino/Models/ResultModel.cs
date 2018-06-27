using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pazzino.Models
{
    public class ResultModel
    {
        public String StringResult { get; set; }
        public bool BooleanResult { get; set; }
        public int IntegerResult { get; set; }
        public List<int> IntegerList { get; set; }
        public Object ObjectResult { get; set; }
        public string JavascriptResult { get; set; }
        public ResultModel() { }

        public ResultModel(bool boolResult = true, string stringResult = "", int intResult = 0)
        {
            this.BooleanResult = boolResult;
            this.StringResult = stringResult;
            this.IntegerResult = intResult;
        }
        public ResultModel(bool boolResult = true, string stringResult = "")
        {
            this.BooleanResult = boolResult;
            this.StringResult = stringResult;
        }
        public static ResultModel SuccessResult()
        {
            var obj = new ResultModel();
            obj.BooleanResult = true;
            obj.StringResult = "Success!";
            return obj;
        }
        public static ResultModel FailResult()
        {
            var obj = new ResultModel();
            obj.BooleanResult = false;
            obj.StringResult = "Fail!";
            return obj;
        }
    }
}
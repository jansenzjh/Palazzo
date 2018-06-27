using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

public static class Helper
{
    public static DataSourceResult ToUIDataSourceResult<T>(bool isPaged, Object data, DataSourceRequest request, int total)
    {
        if (isPaged)
        {
            request.Page = 1;
        }
        var list = data as List<T>;
        var returnObjs = list.ToDataSourceResult(request);
        returnObjs.Total = total;
        return returnObjs;
    }
    
}

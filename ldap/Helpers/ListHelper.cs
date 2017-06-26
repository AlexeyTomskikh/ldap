namespace ldap.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    using ldap.Models;

    public static class ListHelper
    {
        // метод печатает один час с произвольным количеством событий
        public static MvcHtmlString CellWithArbitraryQuantity(this HtmlHelper html, List<Event>[] items)
        {
            
            // Tags
            TagBuilder tr = new TagBuilder("tr");
            TagBuilder td = new TagBuilder("td");

            // Inner html of table
            StringBuilder sb = new StringBuilder();

            // Add data

            for (int i = 0; i <= 24; i++)
            {
                
            }
            // Ячейка с часом
            td.InnerHtml = "1 час";
            td.MergeAttribute("rowspan", (items[1].Count).ToString()); // rowspan - количество событив часе
            tr.InnerHtml += td.ToString();
            sb.Append(tr);

            // Первое событие
            td.InnerHtml = items[1].ElementAt(1).DescriptionEvent;
            tr.InnerHtml += td.ToString();
            sb.Append(tr);

            foreach (var d in items[1])
            {
                td.InnerHtml = d.DescriptionEvent;
            }

            tr.InnerHtml += td.ToString();
            sb.Append(tr);

            return new MvcHtmlString(sb.ToString());
        }


        //public static MvcHtmlString CreateList(this HtmlHelper html, List<Event> items)
        //{
        //    TagBuilder ul = new TagBuilder("ul");
        //    foreach (var item in items)
        //    {
        //        TagBuilder li = new TagBuilder("li");
        //        li.SetInnerText(item.DescriptionEvent);
        //        ul.InnerHtml += li.ToString();
        //    }

        //    return new MvcHtmlString(ul.ToString());
        //}

        


        // Метод расширения возвращает таблицу с расписанием на день
        //public static MvcHtmlString CreateTable(this HtmlHelper html, List<Event> items)
        //{
        //    // Tags
        //    TagBuilder table = new TagBuilder("table");
        //    TagBuilder tr = new TagBuilder("tr");
        //    TagBuilder td = new TagBuilder("td");
        //    TagBuilder th = new TagBuilder("th");

        //    table.MergeAttribute("border", "1");

            // Inner html of table
            //StringBuilder sb = new StringBuilder();

            // Add headers
             //foreach (var s in items)
             //{
             //   th.InnerHtml = s.DescriptionEvent ;
             //   tr.InnerHtml += th.ToString();
             //}

            //th.InnerHtml = "Время";
            //tr.InnerHtml += th.ToString();
            //th.InnerHtml = "События";
            //tr.InnerHtml += th.ToString();

            //sb.Append(tr);

            // Add data
            //foreach (var d in items)
            //{
            //    tr.InnerHtml = string.Empty;
            //    foreach (var h in items)
            //    {
            //        td.InnerHtml = d.DescriptionEvent;
            //        tr.InnerHtml += td.ToString();
            //    }

            //    sb.Append(tr);
            //}

            // Add data
        //    foreach (var d in items)
        //    {
        //        td.InnerHtml = d.DescriptionEvent;  
        //    }

        //    tr.InnerHtml += td.ToString();
        //    sb.Append(tr);

            

        //    table.InnerHtml = sb.ToString();
        //    return new MvcHtmlString(table.ToString());
        //}
    }
}
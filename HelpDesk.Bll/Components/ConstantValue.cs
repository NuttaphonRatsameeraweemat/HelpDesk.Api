using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Bll.Components
{
    public static class ConstantValue
    {
        //Claims Type
        public const string ClamisNameTh = "NameTh";
        public const string ClamisNameEn = "NameEn";
        //Response Header Content Type Format
        public const string ContentTypeJson = "application/json";
        //UserType
        public const string UserTypeAdmin = "ADMIN";
        public const string UserTypeUser = "USER";
        //Date format
        public const string DateTimeFormat = "yyyy-MM-dd";
        //Template format.
        public const string EmpTemplate = "{0} {1}";
        //Regular expresstion format date
        public const string RegexDateFormat = @"^[0-9]{4}-[0-9]{2}-[0-9]{2}$";
        public const string RegexYearFormat = @"^[0-9]{4}$";
        //Error Log Messages.
        public const string HrEmployeeArgumentNullExceptionMessage = "The {0} hasn't in Customer Table.";
        public const string HttpRequestFailedMessage = "Response StatusCode {0}, {1}";
        public const string DateIncorrectFormat = "The date value can't be empty and support only 'yyyy-MM-dd' format.";
        public const string YearIncorrectFormat = "The year value can't be empty and support only 'yyyy' format.";
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Bll.Components
{
    public static class ConstantValue
    {
        //Claims Type
        public const string ClamisComCode = "ComCode";
        public const string ClamisUserType = "UserType";
        public const string ClamisFullName = "FullName";
        //Response Header Content Type Format
        public const string ContentTypeJson = "application/json";
        //Redis Keys
        public const string TicketInfoKey = "Ticket";
        public const string TicketTransectionKey = "TicketTransection";
        public const string TicketCommentKey = "TicketComment";
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
        public const string EmailCannotSending = "Can't send email.";
        //Ticket Status
        public const string TicketStatusOpen = "OPEN";
        public const string TicketStatusWaiting = "WAITING";
        public const string TicketStatusClose = "CLOSE";
        //Email Status
        public const string EmailSendingComplete = "SEND";
        public const string EmailSendingError = "ERROR";
        //Email Template
        public const string EmailForgetPasswordSubject = "Your New Password, Leaderplanet TicketIssue System.";
        public const string EmailForgetPasswordBody =
        @"Dear {0} <br /> 
        You have requested a password reset, your new password is {1}. <br />
        Please contact admin if you did not request a password change.";
        public const string EmailNotificationNewTicketSubject = "Notification new issue ticket {0} from {1}";
        public const string EmailNotificationNewTicketBody =
        @"Dear SupportTeam <br /> 
        New Ticket issue from {0}. <br />
        Ticket No : {1}. <br />
        Ticket Name : {2}. <br />
        Ticket Description : {3}. <br />
        Ticket Priority : {4}. <br />";
        public const string EmailNotificationUpdateTicketSubject = "Notification ticket issue {0} update status to {1}";
        public const string EmailNotificationUpdateTicketBody =
        @"Dear {0} <br /> 
        Ticket No : {1}. <br />
        Ticket Name : {2}. <br />
        Ticket Description : {3}. <br />
        Ticket Priority : {4}. <br />
        Status change to {5}, <br />
        CommentBy : {6}, <br />
        Comment : {7}. <br />";
    }
}

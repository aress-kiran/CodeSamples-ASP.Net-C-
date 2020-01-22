// ***********************************************************************
// Assembly         : SRDM-BLL
// Author           : Aress
// Created          : 10-07-2015
//
// Last Modified By : Aress
// Last Modified On : 10-07-2015
// ***********************************************************************
// <copyright file="SRDMAPIHelper.cs" company="Aress">
//     Copyright © Microsoft 2015
// </copyright>
// <summary></summary>
// ***********************************************************************

using Microsoft.Win32;
using Newtonsoft.Json;
using SRDM_BLL.Enumeration;
using SRDM_BLL.Log4Net;
using SRDM_BLL.Models;
using SRDM_BLL.SkyLink;
using SRDM_BLL.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/// <summary>
/// The Common namespace.
/// </summary>
namespace SRDM_BLL.Common
{
    /// <summary>
    /// Class SRDMAPIHelper.
    /// </summary>
    public static class SRDMAPIHelper
    {
        #region -- Variable/Object Declaration --

        const string urlCheckLogin = "api/SRDM/CheckLogin";
        const string urlGetProperties = "api/SRDM/GetProperties";
        const string urlGetProfitcentersByProperty = "api/SRDM/GetProfitcentersByProperty";
        const string urlGetProfitcenters = "api/SRDM/GetProfitcenters";
        const string urlGetReportNames = "api/SRDM/GetReportNames";
        const string urlExecuteReport = "api/SRDM/ExecuteReport";
        const string urlSendEmail = "api/SRDM/SendEmail";
        static ILogger logger = new Logger(typeof(SRDMAPIHelper));


        public static SRDMEntities db;

        #endregion

        #region -- API Calls --

        /// <summary>
        /// Gets the report names.
        /// </summary>
        /// <param name="strSRDMApiURL">The string SRDM API URL.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static Dictionary<string, string> GetReportNames(string strSRDMApiURL)
        {
            try
            {
                ServiceResponse objResponse = new ServiceResponse();
                Dictionary<string, string> list = new Dictionary<string, string>();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(string.Concat(strSRDMApiURL, urlGetReportNames)).Result;

                if (response.IsSuccessStatusCode)
                {
                    objResponse = response.Content.ReadAsAsync<ServiceResponse>().Result;
                }

                list = JsonConvert.DeserializeObject<Dictionary<string, string>>(objResponse.Result.ToString());
                return list;
            }
            catch (Exception)
            {
                return new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// Execute Report
        /// </summary>
        /// <param name="reportParameters"></param>
        /// <param name="strSRDMApiURL"></param>
        /// <returns></returns>
        public static async Task<ServiceResponse> ExecuteReport(ReportViewModel reportParameters, string strSRDMApiURL)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    ServiceResponse objResponse = new ServiceResponse();
                    HttpClient client = new HttpClient();

                    // Create the JSON formatter.
                    MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();

                    // Use the JSON formatter to create the content of the request body.
                    HttpContent content = new ObjectContent<ReportViewModel>(reportParameters, jsonFormatter);

                    // Send the request.
                    HttpResponseMessage response = await client.PostAsync(string.Concat(strSRDMApiURL, urlExecuteReport), content);


                    if (response.IsSuccessStatusCode)
                    {
                        objResponse = response.Content.ReadAsAsync<ServiceResponse>().Result;
                    }


                    objResponse.Message = response.ReasonPhrase + " : " + response.StatusCode + " : " + response.Content.ReadAsStringAsync().Result;

                    return objResponse;
                }
                catch (Exception)
                {
                    return new ServiceResponse();
                }

            }
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <param name="strSRDMApiURL">The string SRDM API URL.</param>
        /// <param name="company">The company.</param>
        /// <returns>List&lt;KeyValue&gt;.</returns>
        public static List<KeyValue> GetProperties(string strSRDMApiURL, string company)
        {
            try
            {
                ServiceResponse objResponse = new ServiceResponse();
                List<KeyValue> list = new List<KeyValue>();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(string.Concat(strSRDMApiURL, string.Concat(urlGetProperties, "?company=", company))).Result;

                if (response.IsSuccessStatusCode)
                {
                    objResponse = response.Content.ReadAsAsync<ServiceResponse>().Result;
                }
                list = JsonConvert.DeserializeObject<List<KeyValue>>(objResponse.Result.ToString());
                return list;
            }
            catch (Exception)
            {
                return new List<KeyValue>();
            }
        }

        /// <summary>
        /// Gets the profit centers by property.
        /// </summary>
        /// <param name="strSRDMApiURL">The string SRDM API URL.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>List&lt;KeyValue&gt;.</returns>
        public static List<KeyValue> GetProfitcentersByProperty(string strSRDMApiURL, string connectionString, string id)
        {
            try
            {
                ServiceResponse objResponse = new ServiceResponse();
                List<KeyValue> list = new List<KeyValue>();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(string.Concat(strSRDMApiURL, string.Concat(urlGetProfitcentersByProperty, "?connectionString=", connectionString, "&id=", id))).Result;

                if (response.IsSuccessStatusCode)
                {
                    objResponse = response.Content.ReadAsAsync<ServiceResponse>().Result;
                }
                list = JsonConvert.DeserializeObject<List<KeyValue>>(objResponse.Result.ToString());
                return list;
            }
            catch (Exception)
            {
                return new List<KeyValue>();
            }
        }

        /// <summary>
        /// Gets the profit centers.
        /// </summary>
        /// <param name="strSRDMApiURL">The string SRDM API URL.</param>
        /// <param name="company">The company.</param>
        /// <returns>List&lt;KeyValue&gt;.</returns>
        public static List<KeyValue> GetProfitcenters(string strSRDMApiURL, string company)
        {
            try
            {
                ServiceResponse objResponse = new ServiceResponse();
                List<KeyValue> list = new List<KeyValue>();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(string.Concat(strSRDMApiURL, string.Concat(urlGetProfitcenters, "?company=", company))).Result;

                if (response.IsSuccessStatusCode)
                {
                    objResponse = response.Content.ReadAsAsync<ServiceResponse>().Result;
                }
                list = JsonConvert.DeserializeObject<List<KeyValue>>(objResponse.Result.ToString());
                return list;
            }
            catch (Exception)
            {
                return new List<KeyValue>();
            }
        }

        /// <summary>
        /// Gets the connectionstring for synchronize.
        /// </summary>
        /// <param name="dbName">Name of the database.</param>
        /// <returns>System.String.</returns>
        public static string GetConnectionstringForSync(string dbName)
        {
            string strConn = string.Empty;

            strConn = System.Configuration.ConfigurationManager.ConnectionStrings["SampleConnection"].ConnectionString.Replace("#DB#", dbName);

            return strConn;
        }

        /// <summary>
        /// Prints string
        /// </summary>
        /// <param name="s"></param>
        public static string PrintJob(string pinterName, string byteArray)
        {
            string s = byteArray;

            PrintDocument p = new PrintDocument();
            p.PrinterSettings.PrinterName = pinterName;
            p.PrintPage += delegate(object sender1, PrintPageEventArgs e1)
            {
                e1.Graphics.DrawString(s, new Font("Times New Roman", 12), new SolidBrush(Color.Black), new RectangleF(0, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
            };
            try
            {
                p.Print();
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Checks the login.
        /// </summary>
        /// <param name="objLoginView">The object login view.</param>
        /// <param name="strSRDMApiURL">The string SRDM API URL.</param>
        /// <param name="password">The password.</param>
        /// <returns>ServiceResponse.</returns>
        public static ServiceResponse CheckLogin(LoginViewModel objLoginView, string strSRDMApiURL, string password)
        {
            try
            {
                ServiceResponse objResponse = new ServiceResponse();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(string.Concat(strSRDMApiURL, string.Concat(urlCheckLogin, "?UserName=", objLoginView.UserName, "&Company=", objLoginView.Company, "&password=", password))).Result;

                if (response.IsSuccessStatusCode)
                {
                    objResponse = response.Content.ReadAsAsync<ServiceResponse>().Result;
                }
                return objResponse;
            }
            catch (Exception)
            {
                return new ServiceResponse();
            }
        }
        #endregion

        /// <summary>
        /// Gets the time difference.
        /// </summary>
        /// <param name="selectedTimeZone">The selected time zone.</param>
        /// <param name="time">The time.</param>
        /// <returns>TimeSpan.</returns>
        public static TimeSpan GetTimeDifference(string selectedTimeZone, DateTime time)
        {
            TimeZone curTimeZone = TimeZone.CurrentTimeZone;

            var currentTZ = TimeZoneInfo.FindSystemTimeZoneById(curTimeZone.StandardName);
            var selectedTZ = TimeZoneInfo.FindSystemTimeZoneById(selectedTimeZone);

            var now = time;
            TimeSpan currentOffset = currentTZ.GetUtcOffset(now);
            TimeSpan selectedOffset = selectedTZ.GetUtcOffset(now);
            TimeSpan difference = currentOffset - selectedOffset;
            return difference;
        }

        /// <summary>
        /// Converts to local time.
        /// </summary>
        /// <param name="selectedDate">The selected date.</param>
        /// <param name="selectedTimeZone">The selected time zone.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ConvertToLocalTime(DateTime selectedDate, string selectedTimeZone)
        {
            var selectedTimezone = TimeZoneInfo.FindSystemTimeZoneById(selectedTimeZone);
            var selectedUtcTime = TimeZoneInfo.ConvertTimeToUtc(selectedDate, selectedTimezone);
            var localTime = selectedUtcTime.ToLocalTime();

            return localTime;
        }

        /// <summary>
        /// Converts from local time.
        /// </summary>
        /// <param name="localDatetime">The local datetime.</param>
        /// <param name="toTimeZone">To time zone.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ConvertFromLocalTime(DateTime localDatetime, string toTimeZone)
        {
            var localToUtcTime = localDatetime.ToUniversalTime();
            var selectedTimezone = TimeZoneInfo.FindSystemTimeZoneById(toTimeZone);
            var ConvertedZoneTime = TimeZoneInfo.ConvertTimeFromUtc(localToUtcTime, selectedTimezone);

            return ConvertedZoneTime;
        }

        /// <summary>
        /// Adds the next run time.
        /// </summary>
        /// <param name="timePeriod">The time period.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <param name="StartdateTime">The startdate time.</param>
        /// <param name="EnddateTime">The enddate time.</param>
        /// <param name="weeklyDay">The weekly day.</param>
        /// <param name="runTime">The run time.</param>
        /// <returns>DateTime.</returns>
        public static DateTime AddNextRunTime(int timePeriod, string timeZone, DateTime StartdateTime, DateTime EnddateTime, string weeklyDay, DateTime runTime)
        {
            DateTime selectedDateTime;
            DateTime selectedEndDateTime;
            DateTime localDateTime;
            DateTime nextRunTime = DateTime.Now;
            var hours = 0.0;
            switch (timePeriod)
            {
                case (int)Enumerations.TimePeriod.Daily:
                    selectedDateTime = StartdateTime; //  GetTimeZoneDifference(timeZone, StartdateTime);
                    localDateTime = GetTimeZoneDifference(timeZone, DateTime.Now);
                    hours = (localDateTime - selectedDateTime).TotalHours;
                    if (hours > 0)
                        nextRunTime = selectedDateTime.AddDays(1);
                    else
                        nextRunTime = selectedDateTime;
                    break;
                case (int)Enumerations.TimePeriod.Hourly:
                    selectedDateTime = StartdateTime; // GetTimeZoneDifference(timeZone, StartdateTime);
                    selectedEndDateTime = EnddateTime; // GetTimeZoneDifference(timeZone, EnddateTime);
                    localDateTime = GetTimeZoneDifference(timeZone, DateTime.Now);

                    string strattime = selectedDateTime.ToString("HH:mm:ss");
                    string endtime = selectedEndDateTime.ToString("HH:mm:ss");
                    TimeSpan startTimeSpan = TimeSpan.Parse(strattime);
                    TimeSpan endTimeSpan = TimeSpan.Parse(endtime);
                    bool result = false;

                    result = BeforeTime(localDateTime, startTimeSpan);

                    if (result)
                    {
                        string starttime = strattime;
                        DateTime startdate = selectedDateTime;
                        DateTime startdtTime = startdate;
                        startdtTime = Convert.ToDateTime(starttime);
                        string strstartdTime = startdtTime.ToString("HH:mm:ss");
                        DateTime startCombinedDate = startdtTime;
                        nextRunTime = startCombinedDate;
                    }
                    else
                    {
                        result = TimeBetween(localDateTime, startTimeSpan, endTimeSpan);

                        if (result)
                        {
                            var timeOfDay = localDateTime.TimeOfDay;
                            var nextFullHour = TimeSpan.FromHours(Math.Ceiling(timeOfDay.TotalHours));
                            string time = Convert.ToString(nextFullHour);
                            DateTime date = localDateTime;
                            DateTime dtTime = date;
                            dtTime = Convert.ToDateTime(time);

                            int minutes = startTimeSpan.Minutes; // .ToString("mm");
                            dtTime = dtTime.AddMinutes(Convert.ToDouble(minutes));

                            nextRunTime = dtTime;
                        }
                        else
                        { nextRunTime = selectedDateTime.AddDays(1); }
                    }
                    break;
                case (int)Enumerations.TimePeriod.Weekly:
                    selectedDateTime = GetNextWeekday(runTime, GetDayofWeek(weeklyDay)); // runTime; //GetTimeZoneDifference(timeZone, runTime);
                    localDateTime = GetTimeZoneDifference(timeZone, DateTime.Now);

                    TimeSpan difference = selectedDateTime - localDateTime;
                    double dayDiff = difference.TotalDays;

                    if (dayDiff < 0)
                    {
                        hours = (localDateTime - selectedDateTime).TotalHours;
                        if (hours > 0)
                            nextRunTime = selectedDateTime.AddDays(7);
                        else
                            nextRunTime = selectedDateTime;
                    }
                    else
                    {
                        var weekdays = (localDateTime - selectedDateTime).TotalDays;
                        if (weekdays > 0)
                            nextRunTime = GetNextWeekday(runTime, GetDayofWeek(weeklyDay));
                        else
                            nextRunTime = selectedDateTime;
                    }
                    break;
                case (int)Enumerations.TimePeriod.Bi_Weekly:
                    selectedDateTime = runTime; // GetTimeZoneDifference(timeZone, runTime);
                    localDateTime = GetTimeZoneDifference(timeZone, DateTime.Now);
                    hours = (localDateTime - selectedDateTime).TotalHours;
                    var days = (localDateTime - selectedDateTime).TotalDays;
                    if (hours > 0)
                        nextRunTime = selectedDateTime.AddDays(15);
                    else
                        nextRunTime = selectedDateTime;
                    break;
                case (int)Enumerations.TimePeriod.Monthly:
                    selectedDateTime = runTime; // GetTimeZoneDifference(timeZone, runTime);
                    localDateTime = GetTimeZoneDifference(timeZone, DateTime.Now);
                    hours = (localDateTime - selectedDateTime).TotalHours;
                    if (hours > 0)
                        nextRunTime = selectedDateTime.AddMonths(1);
                    else
                        nextRunTime = selectedDateTime;
                    break;
                case (int)Enumerations.TimePeriod.Annually:
                    selectedDateTime = runTime; // GetTimeZoneDifference(timeZone, runTime);
                    localDateTime = GetTimeZoneDifference(timeZone, DateTime.Now);
                    hours = (localDateTime - selectedDateTime).TotalHours;
                    if (hours > 0)
                        nextRunTime = selectedDateTime.AddYears(1);
                    else
                        nextRunTime = selectedDateTime;
                    break;
            }
            TimeZone objTimezone = TimeZone.CurrentTimeZone;
            return GetTimeZoneDifference(timeZone, objTimezone.StandardName, nextRunTime);
        }

        /// <summary>
        /// Times the between.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        static bool TimeBetween(DateTime datetime, TimeSpan start, TimeSpan end)
        {
            // convert datetime to a TimeSpan
            TimeSpan now = datetime.TimeOfDay;
            // see if start comes before end
            if (start < end)
                return start <= now && now <= end;
            // start is after end, so do the inverse comparison
            return !(end < now && now < start);
        }

        static bool BeforeTime(DateTime datetime, TimeSpan start)
        {
            // convert datetime to a TimeSpan
            TimeSpan now = datetime.TimeOfDay;
            // see if start comes before end
            if (now < start)
                return true;
            // start is after end, so do the inverse comparison
            return false;
        }

        /// <summary>
        /// Get start date time.
        /// </summary>
        /// <param name="timePeriod">The time period.</param>       
        /// <param name="runTime">The EnddateTime.</param>
        /// <returns>DateTime.</returns>
        public static string GetStartDateTime(int timePeriod, int period, string timezone, out DateTime endDate)
        {
            DateTime StartDateTime = DateTime.Now; ;
            endDate = StartDateTime;
            switch (period)
            {
                case (int)Enumerations.Period.CurrentDay:
                    StartDateTime = GetTimeZoneDifference(timezone, DateTime.Now);
                    endDate = StartDateTime;
                    break;
                case (int)Enumerations.Period.PreviousDay:
                    StartDateTime = GetTimeZoneDifference(timezone, DateTime.Now);
                    StartDateTime = StartDateTime.AddDays(-1);
                    endDate = StartDateTime;
                    break;
                case (int)Enumerations.Period.LastWeek:
                    StartDateTime = GetTimeZoneDifference(timezone, DateTime.Now);
                    endDate = StartDateTime;
                    break;
                case (int)Enumerations.Period.LastMonth:
                    var today = GetTimeZoneDifference(timezone, DateTime.Now);
                    var month = new DateTime(today.Year, today.Month, 1);
                    var first = month.AddMonths(-1);
                    var last = month.AddDays(-1);
                    StartDateTime = first;
                    endDate = last;
                    break;
                case (int)Enumerations.Period.LastYear:
                    int year = (DateTime.Now.Year - 1);
                    DateTime firstDay = new DateTime(year, 1, 1);
                    DateTime lastDay = new DateTime(year, 12, 31);
                    StartDateTime = firstDay;
                    endDate = lastDay;
                    break;
                case (int)Enumerations.Period.WeekToDate:
                    StartDateTime = GetTimeZoneDifference(timezone, DateTime.Now);
                    endDate = StartDateTime;
                    break;
                case (int)Enumerations.Period.MonthToDate:
                    DateTime fDayM = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    StartDateTime = fDayM;
                    endDate = GetTimeZoneDifference(timezone, DateTime.Now);
                    break;
                case (int)Enumerations.Period.YearToDate:
                    int cyear = DateTime.Now.Year;
                    DateTime fDay = new DateTime(cyear, 1, 1);
                    StartDateTime = fDay;
                    endDate = GetTimeZoneDifference(timezone, DateTime.Now);
                    break;
            }

            return StartDateTime.ToShortDateString();
        }

        /// <summary>
        /// Gets the day of week.
        /// </summary>
        /// <param name="weeklyDay">The weekly day.</param>
        /// <returns>DayOfWeek.</returns>
        private static DayOfWeek GetDayofWeek(string weeklyDay)
        {
            switch (weeklyDay)
            {
                case "Sunday":
                    return DayOfWeek.Sunday;
                case "Monday":
                    return DayOfWeek.Monday;
                case "Tuesday":
                    return DayOfWeek.Tuesday;
                case "Wednesday":
                    return DayOfWeek.Wednesday;
                case "Thursday":
                    return DayOfWeek.Thursday;
                case "Friday":
                    return DayOfWeek.Friday;
                case "Saturday":
                    return DayOfWeek.Saturday;
                default:
                    return DayOfWeek.Sunday;
            }
        }

        /// <summary>
        /// Gets the next weekday.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="day">The day.</param>
        /// <returns>DateTime.</returns>
        private static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }

        /// <summary>
        /// Gets the time zone difference.
        /// </summary>
        /// <param name="timeZone">The time zone.</param>
        /// <param name="dateTime">The date time.</param>
        /// <returns>DateTime.</returns>
        private static DateTime GetTimeZoneDifference(string timeZone, DateTime dateTime)
        {
            TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            DateTime targetTime = TimeZoneInfo.ConvertTime(dateTime, est);

            return targetTime;
        }

        /// <summary>
        /// Gets the assembly title.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns>System.String.</returns>
        public static string GetAssemblyTitle(Assembly app)
        {
            AssemblyTitleAttribute title = (AssemblyTitleAttribute)app.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0];
            return title.Title;
        }

        /// <summary>
        /// Gets the assembly version.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns>System.String.</returns>
        public static string GetAssemblyVersion(Assembly app)
        {
            string version = string.Concat(app.GetName().Version.Major.ToString(), ".", app.GetName().Version.Minor.ToString());
            //  return app.GetName().Version.MajorRevision.ToString();

            return version;
        }

        /// <summary>
        /// Gets the local ip address.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetLocalIPAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
        }

        /// <summary>
        /// Updates the next run time job.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        public static void UpdateNextRunTimeJob(int jobId)
        {
            db = new SRDMEntities();
            Job obj = new Job();

            try
            {
                TimeZone objTimezone = TimeZone.CurrentTimeZone;
                //  

                obj = (from jb in db.Jobs
                       where jb.id == jobId
                       select jb).SingleOrDefault();

                DateTime selectedDateTime;
                DateTime selectedEndDateTime;
                DateTime nextRunTime = obj.NextRunTime;
                //  var hours = 0.0;
                switch (obj.TimePeriod)
                {
                    case (int)Enumerations.TimePeriod.Daily:
                        obj.NextRunTime = nextRunTime.AddDays(1);
                        break;
                    case (int)Enumerations.TimePeriod.Hourly:
                        selectedDateTime = nextRunTime;
                        selectedEndDateTime = nextRunTime.AddHours(1);

                        string startParam = obj.StartParam;
                        string[] startValue = startParam.Split('#');

                        string starttime = startValue[0];
                        DateTime startdate = selectedDateTime;
                        DateTime startdtTime = startdate;
                        startdtTime = Convert.ToDateTime(starttime);
                        string strstartdTime = startdtTime.ToString("HH:mm:ss");
                        DateTime startCombinedDate = GetTimeZoneDifference(obj.TimeZone, objTimezone.StandardName, startdtTime);

                        string time = startValue[1];
                        DateTime date = selectedDateTime;
                        DateTime dtTime = date;
                        dtTime = Convert.ToDateTime(time);
                        string strTime = dtTime.ToString("HH:mm:ss");
                        DateTime combinedDate = GetTimeZoneDifference(obj.TimeZone, objTimezone.StandardName, dtTime);

                        var endhours = (combinedDate - selectedEndDateTime).TotalHours;
                        if (endhours > 0)
                            nextRunTime = nextRunTime.AddHours(1);
                        else
                        {
                            if (selectedEndDateTime == combinedDate)
                                nextRunTime = selectedEndDateTime;
                            else
                                nextRunTime = startCombinedDate.AddDays(1);
                        }

                        obj.NextRunTime = nextRunTime;
                        break;
                    case (int)Enumerations.TimePeriod.Weekly:
                        obj.NextRunTime = nextRunTime.AddDays(1);
                        break;
                    case (int)Enumerations.TimePeriod.Bi_Weekly:
                        obj.NextRunTime = nextRunTime.AddDays(15);
                        break;
                    case (int)Enumerations.TimePeriod.Monthly:
                        obj.NextRunTime = nextRunTime.AddMonths(1);
                        break;
                    case (int)Enumerations.TimePeriod.Annually:
                        obj.NextRunTime = nextRunTime.AddYears(1);
                        break;
                }

                db.SaveChanges();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="emailPhone">The email phone.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool SendEmail(string emailPhone, string subject, string body)
        {
            MailMessage message = new MailMessage();
            foreach (var address in emailPhone.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                message.To.Add(address);
            }
            MailAddress fromMail = new MailAddress(ConfigurationManager.AppSettings["fromMailId"]);
            message.From = fromMail;
            message.Subject = subject;
            message.Body = body;

            Stream stream = new MemoryStream(ASCIIEncoding.ASCII.GetBytes(message.Body));
            AlternateView alternate = new AlternateView(stream, new System.Net.Mime.ContentType("text/html"));
            message.AlternateViews.Add(alternate);
            SmtpClient client = GetSmtpClient();
            client.Send(message);

            return true;

        }

        /// <summary>
        /// Gets the SMTP client.
        /// </summary>
        /// <returns>SmtpClient.</returns>
        public static SmtpClient GetSmtpClient()
        {
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["smtpClient"]);
            client.Credentials = CredentialCache.DefaultNetworkCredentials;
            client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["ClientPort"]);

            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["fromMailId"], ConfigurationManager.AppSettings["fromMailIdPassword"].ToString());
            return client;
        }

        /// <summary>
        /// Gets the required jobs.
        /// </summary>
        /// <param name="isFromScheduler">if set to <c>true</c> [is from scheduler].</param>
        /// <param name="jobId">The job identifier.</param>
        /// <exception cref="System.Exception">Could not found Adobe Reader on the system to execute print job </exception>
        public static void GetRequiredJobs(bool isFromScheduler, int jobId = 0)
        {
            string getDefaultPrinter = string.Empty;
            User user = new User();
            List<Job> lstJobs = new List<Job>();
            List<Job> lstSchedulerJob = new List<Job>();
            Destination objDestination = new Destination();
            Job objJobs = new Job();
            List<JobSection> objJobSection = new List<JobSection>();
            List<JobReport> objJobReport = new List<JobReport>();
            List<JobSM> objJobSM = new List<JobSM>();
            ReportViewModel reportParameters = new ReportViewModel();
            SRDMEntities db = new SRDMEntities();
            TimeZone objTimezone = TimeZone.CurrentTimeZone;
            DateTime dt = DateTime.Now;
            string logMsg = string.Empty;
            bool isSpecificDay = false;
            try
            {
                if (isFromScheduler)
                {
                    dt = new DateTime(dt.Ticks - (dt.Ticks % TimeSpan.TicksPerMinute), dt.Kind);
                    lstJobs = db.Jobs.Where(x => x.NextRunTime == dt && x.IsDeleted == false && x.IsEnabled == true).ToList();
                    UpdateNextRunTimeforDisableJobs(dt, db);
                }
                else
                {
                    lstJobs = db.Jobs.Where(x => x.id == jobId && x.IsDeleted == false).ToList(); //&& x.IsEnabled == true
                }

                if (lstJobs.Count > 0)
                    logMsg = string.Concat(" Job Ids : ", String.Join(",", lstJobs.Select(x => x.id).ToArray()));

                var actionInfo = new SRDM_BLL.Log4Net.ActionLoggerInfo()
                {
                    Module = Enumerations.GetDescription(Enumerations.Modules.JobExecution),
                    DateTime = DateTime.Now,
                    Status = (int)SRDM_BLL.Enumeration.Enumerations.Status.Success,
                    DestinationType = Enumerations.GetDescription((SRDM_BLL.Enumeration.Enumerations.DestinationType)objDestination.DestinationType),
                    CompanyName = user.Company,
                    Message = string.Concat("Executing Jobs: Count (", lstJobs.Count, ").", logMsg),
                    UserId = user.id,
                    WebAddress = user.WebAddress,
                    IPAddress = SRDMAPIHelper.GetLocalIPAddress()
                };
                logger.LogError(actionInfo);

                if (lstJobs.Count > 0)
                {
                    foreach (Job objJob in lstJobs)
                    {
                        if (isFromScheduler)
                            isSpecificDay = CheckSpecificDayRun(objJob.SpecificDays);
                        else
                            isSpecificDay = true;

                        if (isFromScheduler)
                        {
                            SRDMAPIHelper.UpdateNextRunTimeJob(objJob.id);
                        }

                        if (isSpecificDay)
                        {
                            objDestination = db.Destinations.Where(x => x.id == objJob.DestinationId).SingleOrDefault();

                            objJobs = db.Jobs.Where(j => j.DestinationId == objJob.DestinationId && j.id == objJob.id).SingleOrDefault();

                            if (objDestination.DestinationType == (int)Enumerations.DestinationType.Printer || objDestination.DestinationType == (int)Enumerations.DestinationType.Email)
                                objJobReport = db.JobReports.Where(x => x.jobId == objJob.id).ToList();
                            else
                                objJobSM = db.JobSMS.Where(x => x.jobId == objJob.id).ToList();

                            user = db.Users.Where(x => x.id == objDestination.Uid).SingleOrDefault();

                            reportParameters.Company = user.Company;
                            reportParameters.UserName = user.Name;


                            DateTime endDate = DateTime.Now;
                            reportParameters.Period = objJob.Period;
                            reportParameters.FromDate = Convert.ToString(SRDMAPIHelper.GetStartDateTime(objJob.TimePeriod, objJob.Period, objJob.TimeZone, out endDate));
                            reportParameters.Todate = Convert.ToString(endDate.ToShortDateString());
                            reportParameters.TimeZone = objJobs.TimeZone;
                            reportParameters.TimePeriod = Enumerations.GetDescription((Enumerations.TimePeriod)objJobs.TimePeriod);
                            reportParameters.UserList = string.Empty;
                            reportParameters.TerminalList = string.Empty;
                            reportParameters.DestinationType = objDestination.DestinationType;
                            reportParameters.UserName = user.Name;
                            reportParameters.DestinationName = objDestination.DestinationName;
                            reportParameters.JobId = objJobs.id;

                            #region  -- Printer And Email Job Execution --

                            if ((objDestination.DestinationType == (int)Enumerations.DestinationType.Email) || (objDestination.DestinationType == (int)Enumerations.DestinationType.Printer))
                            {
                                foreach (JobReport reports in objJobReport)
                                {
                                    List<KeyValue> properties = new List<KeyValue>();
                                    List<KeyValue> profitCenters = new List<KeyValue>();

                                    if (reports.ReportSection == (int)Enumerations.ReportSection.System)
                                    {
                                        properties = null;
                                        profitCenters = null;
                                        reportParameters.level = 2;
                                    }
                                    if (reports.ReportSection == (int)Enumerations.ReportSection.Property)
                                    {
                                        objJobSection = db.JobSections.Where(x => x.ReportId == reports.id).ToList(); //&& x.sectionType == objJob.ReportSection
                                        foreach (JobSection jobSection in objJobSection)
                                        {
                                            KeyValue P = new KeyValue();
                                            P.Id = jobSection.SectionId;
                                            reportParameters.level = 1;
                                            reportParameters.locationId = P.Id;
                                            properties.Add(P);
                                        }
                                    }
                                    if (reports.ReportSection == (int)Enumerations.ReportSection.ProfitCenter)
                                    {
                                        objJobSection = db.JobSections.Where(x => x.ReportId == reports.id).ToList(); //&& x.sectionType == objJob.ReportSection
                                        foreach (JobSection jobSection in objJobSection)
                                        {
                                            KeyValue PC = new KeyValue();
                                            PC.Id = jobSection.SectionId;
                                            reportParameters.level = 0;
                                            reportParameters.locationId = PC.Id;
                                            profitCenters.Add(PC);
                                        }
                                    }
                                    reportParameters.ReportName = reports.ReportId;

                                    if (reports.ReportType == -1)
                                    {
                                        reportParameters.ReportType = reports.ReportSection;
                                    }
                                    else
                                    {
                                        reportParameters.ReportType = reports.ReportType;
                                    }

                                    if (objDestination.DestinationType == (int)Enumerations.DestinationType.Email)
                                        reportParameters.EmailAddress = objDestination.DestinationPath;
                                    if (objDestination.DestinationType == (int)Enumerations.DestinationType.SMS)
                                        reportParameters.Phone = objDestination.DestinationPath;

                                    reportParameters.ProfitCenters = profitCenters;
                                    reportParameters.properties = properties;

                                    ServiceResponse result = new ServiceResponse();
                                    result = System.Threading.Tasks.Task.Run(() => SRDMAPIHelper.ExecuteReport(reportParameters, user.WebAddress)).Result;

                                    #region  -- Printer Job Execution --
                                    if (objDestination.DestinationType == (int)Enumerations.DestinationType.Printer)
                                    {
                                        string tempFile = string.Empty;

                                        try
                                        {
                                            string result1 = (string)result.Result;
                                            string result2 = result1.Replace("\"", string.Empty);
                                            byte[] responseBytes = Convert.FromBase64String(result2);

                                            tempFile = AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\JobReport-" + DateTime.Now.ToString("yyyyMMddhhmm") + ".pdf";
                                            System.IO.File.WriteAllBytes(tempFile, responseBytes);

                                            string pdfArguments = string.Format("/N /T \"{0}\" \"{1}\"", tempFile, objDestination.DestinationPath);

                                            getDefaultPrinter = GetDefaultPrinter();
                                            //Get Adobe Registry Path
                                            var adobe = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("App Paths").OpenSubKey("AcroRd32.exe");
                                            if (!string.IsNullOrEmpty(Convert.ToString(adobe)))
                                            {
                                                var path = adobe.GetValue("");
                                                string pdfPrinterLocation = path.ToString();

                                                myPrinters.SetDefaultPrinter(objDestination.DestinationPath);

                                                if (isFromScheduler)
                                                {
                                                    ProcessStarter processStarter = new ProcessStarter("AcroRd32", pdfPrinterLocation, pdfArguments);
                                                    processStarter.Run();
                                                    processStarter.WaitForExit();
                                                    processStarter.Stop();
                                                }
                                                else
                                                {
                                                    KillAdobe("AcroRd32");
                                                    Process proc = new Process();
                                                    proc.StartInfo.FileName = pdfPrinterLocation;
                                                    proc.StartInfo.WindowStyle = ProcessWindowStyle.Normal; //Hide the window.
                                                    //proc.StartInfo.Verb = "PrintTo";
                                                    proc.StartInfo.Arguments = pdfArguments;
                                                    proc.StartInfo.CreateNoWindow = false;
                                                    proc.Start();
                                                    proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                                    if (proc.HasExited == false)
                                                    {
                                                        proc.WaitForExit(10000);
                                                    }

                                                    proc.EnableRaisingEvents = true;
                                                    proc.CloseMainWindow();
                                                    KillAdobe("AcroRd32");
                                                }
                                            }
                                            else
                                            {
                                                actionInfo = new SRDM_BLL.Log4Net.ActionLoggerInfo()
                                                {
                                                    Module = Enumerations.GetDescription(Enumerations.Modules.JobExecution),
                                                    DateTime = DateTime.Now,
                                                    Status = (int)SRDM_BLL.Enumeration.Enumerations.Status.Success,
                                                    DestinationType = Enumerations.GetDescription((SRDM_BLL.Enumeration.Enumerations.DestinationType)objDestination.DestinationType),
                                                    CompanyName = user.Company,
                                                    Message = string.Concat("Print job sent successfully for destination:\n - Destination ID: ", objDestination.id, "\n - Job ID: ",
                                                    objJobs.id, "\n - Origination: ", reportParameters.Company, "\n - Destination Name: ", reportParameters.DestinationName,
                                                    "\n - Job Destination: ", objDestination.DestinationPath, "\n \n Job Parameters",
                                                    "\n - Time Period: ", reportParameters.TimePeriod, "\n - Time Zone: ", reportParameters.TimeZone, "\n - IP Address: ", GetLocalIPAddress(),
                                                    "\n - Report name: ", reportParameters.ReportName, "\n - Error Message: Could not found Adobe Reader on the system to execute print job."),
                                                    UserId = user.id,
                                                    WebAddress = user.WebAddress,
                                                    IPAddress = SRDMAPIHelper.GetLocalIPAddress()
                                                };
                                                logger.LogError(actionInfo);

                                                throw new Exception("Could not found Adobe Reader on the system to execute print job ");
                                            }

                                            actionInfo = new SRDM_BLL.Log4Net.ActionLoggerInfo()
                                            {
                                                Module = Enumerations.GetDescription(Enumerations.Modules.JobExecution),
                                                DateTime = DateTime.Now,
                                                Status = (int)SRDM_BLL.Enumeration.Enumerations.Status.Success,
                                                DestinationType = Enumerations.GetDescription((SRDM_BLL.Enumeration.Enumerations.DestinationType)objDestination.DestinationType),
                                                CompanyName = user.Company,
                                                Message = string.Concat("Print job sent successfully for destination:\n - Destination ID: ", objDestination.id, "\n - Job ID: ",
                                                    objJobs.id, "\n - Origination: ", reportParameters.Company, "\n - Destination Name: ", reportParameters.DestinationName,
                                                    "\n - Job Destination: ", objDestination.DestinationPath, "\n \n Job Parameters",
                                                    "\n - Time Period: ", reportParameters.TimePeriod, "\n - Time Zone: ", reportParameters.TimeZone, "\n - IP Address: ", GetLocalIPAddress(),
                                                    "\n - Report name: ", reportParameters.ReportName),
                                                UserId = user.id,
                                                WebAddress = user.WebAddress,
                                                IPAddress = SRDMAPIHelper.GetLocalIPAddress()
                                            };
                                            logger.LogError(actionInfo);
                                        }
                                        catch (Exception ex)
                                        {
                                            actionInfo = new SRDM_BLL.Log4Net.ActionLoggerInfo()
                                            {
                                                Module = Enumerations.GetDescription(Enumerations.Modules.JobExecution),
                                                DateTime = DateTime.Now,
                                                Status = (int)SRDM_BLL.Enumeration.Enumerations.Status.Failure,
                                                DestinationType = Enumerations.GetDescription((SRDM_BLL.Enumeration.Enumerations.DestinationType)objDestination.DestinationType),
                                                CompanyName = user.Company,
                                                Message = string.Concat("Error occurred while executing PRINT job For destination : \n - Destination ID: ", objDestination.id, "\n - Job ID: ",
                                                    objJobs.id, "\n - Origination: ", reportParameters.Company, "\n - Destination Name: ", reportParameters.DestinationName,
                                                    "\n - Job Destination: ", objDestination.DestinationPath, "\n \n Job Parameters",
                                                    "\n - Time Period: ", reportParameters.TimePeriod, "\n - Time Zone: ", reportParameters.TimeZone, "\n - IP Address: ", GetLocalIPAddress(),
                                                    "\n - Report name: ", reportParameters.ReportName, "\n - Error Message: ", ex.Message),
                                                UserId = user.id,
                                                WebAddress = user.WebAddress,
                                                IPAddress = SRDMAPIHelper.GetLocalIPAddress()
                                            };
                                            logger.LogError(actionInfo);
                                        }
                                        finally
                                        {
                                            if (!string.IsNullOrEmpty(getDefaultPrinter))
                                                myPrinters.SetDefaultPrinter(getDefaultPrinter);

                                            KillAdobe("AcroRd32");
                                            if (!string.IsNullOrEmpty(tempFile))
                                                File.Delete(tempFile);
                                        }
                                    }
                                    #endregion

                                    #region -- Email Job Execution --

                                    else if (objDestination.DestinationType == (int)Enumerations.DestinationType.Email)
                                    {
                                        #region Log Actions
                                        var actionInfo1 = new SRDM_BLL.Log4Net.ActionLoggerInfo()
                                        {
                                            Module = Enumerations.GetDescription(Enumerations.Modules.JobExecution),
                                            DateTime = DateTime.Now,
                                            Status = result.Status,
                                            DestinationType = Enumerations.GetDescription((SRDM_BLL.Enumeration.Enumerations.DestinationType)objDestination.DestinationType),
                                            CompanyName = user.Company,
                                            Message = string.Concat("Email Job Execution: \n - Destination ID: ", objDestination.id, "\n - Job ID: ",
                                                    objJobs.id, "\n - Origination: ", reportParameters.Company, "\n - Destination Name: ", reportParameters.DestinationName,
                                                     "\n - Job Destination: ", objDestination.DestinationPath, "\n \n Job Parameters",
                                                    "\n - Time Period: ", reportParameters.TimePeriod, "\n - Time Zone: ", reportParameters.TimeZone, "\n - IP Address: ", GetLocalIPAddress(),
                                                    "\n - Report name: ", reportParameters.ReportName, "\n - Result Status: ", Enumerations.GetDescription((SRDM_BLL.Enumeration.Enumerations.Status)result.Status), "\n - Result: ", Convert.ToString(result.Result)),
                                            UserId = user.id,
                                            WebAddress = user.WebAddress,
                                            IPAddress = SRDMAPIHelper.GetLocalIPAddress()
                                        };
                                        logger.LogError(actionInfo1);
                                        #endregion

                                        if (result.Status == (int)SRDM_BLL.Enumeration.Enumerations.Status.Failure)
                                        {
                                            StringBuilder body = new StringBuilder(string.Concat("Error occurred while sending report EMAIL to - ", objDestination.DestinationPath));
                                            body.AppendLine("Error Details: " + result.Message);
                                            SRDM_BLL.Common.SRDMAPIHelper.SendEmail(objDestination.DestinationPath, "Job Execution Failure - For Destination " + objDestination.DestinationName, Convert.ToString(body));
                                        }
                                    }
                                    #endregion

                                }
                            }

                            #endregion

                            #region -- SMS Job Execution --

                            if (objDestination.DestinationType == (int)Enumerations.DestinationType.SMS)
                            {
                                List<SMSSection> lstSMSSection = new List<SMSSection>();
                                SMSSection objSMSSection = new SMSSection();
                                List<JobSM> lstJobSMS = new List<JobSM>();
                                lstJobSMS = db.JobSMS.Where(s => s.jobId == objJob.id).ToList();

                                for (int j = 0; j < lstJobSMS.Count; j++)
                                {
                                    switch (lstJobSMS[j].SMSType)
                                    {
                                        case (int)Enumerations.SMSType.GrossSale:
                                            #region -- Gross Sale --
                                            string[] GrossSale = lstJobSMS[j].SMSPeriod.TrimEnd().Split(';');
                                            if (GrossSale.Length != 0)
                                            {
                                                List<SMSPeriod> lstSMSPeriod = new List<SMSPeriod>();
                                                objSMSSection.smsType = (int)Enumerations.SMSType.GrossSale;
                                                int i = 0;
                                                foreach (string id in GrossSale)
                                                {
                                                    switch (i)
                                                    {
                                                        case 0:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.CurrentDay;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.CurrentDay, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                        case 1:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.Yesterday;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.Yesterday, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                        case 2:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.WeekToDate;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.WeekToDate, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                        case 3:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.MonthToDate;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.MonthToDate, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                        case 4:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.YearToDate;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.YearToDate, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                    }
                                                    objSMSSection.smsPeriod = lstSMSPeriod;
                                                }
                                                lstSMSSection.Add(objSMSSection);
                                                objSMSSection = new SMSSection();
                                            }

                                            #endregion
                                            break;
                                        case (int)Enumerations.SMSType.NetSales:
                                            #region -- Net Sale --

                                            string[] NetSale = lstJobSMS[j].SMSPeriod.Split(';');
                                            if (NetSale.Length != 0)
                                            {
                                                List<SMSPeriod> lstSMSPeriod = new List<SMSPeriod>();
                                                objSMSSection.smsType = (int)Enumerations.SMSType.NetSales;
                                                int i = 0;
                                                foreach (string id in NetSale)
                                                {
                                                    switch (i)
                                                    {
                                                        case 0:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.CurrentDay;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.CurrentDay, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                        case 1:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.Yesterday;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.Yesterday, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }

                                                            i++;
                                                            break;
                                                        case 2:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.WeekToDate;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.WeekToDate, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                        case 3:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.MonthToDate;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.MonthToDate, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                        case 4:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.YearToDate;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.YearToDate, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                    }
                                                    objSMSSection.smsPeriod = lstSMSPeriod;
                                                }
                                                lstSMSSection.Add(objSMSSection);
                                                objSMSSection = new SMSSection();
                                            }

                                            #endregion
                                            break;
                                        case (int)Enumerations.SMSType.CheckCount:
                                            #region -- Check Count --

                                            string[] CheckCount = lstJobSMS[j].SMSPeriod.Split(';');
                                            if (CheckCount.Length != 0)
                                            {
                                                List<SMSPeriod> lstSMSPeriod = new List<SMSPeriod>();
                                                objSMSSection.smsType = (int)Enumerations.SMSType.CheckCount;
                                                int i = 0;
                                                foreach (string id in CheckCount)
                                                {
                                                    switch (i)
                                                    {
                                                        case 0:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.CurrentDay;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.CurrentDay, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                        case 1:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.Yesterday;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.Yesterday, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }

                                                            i++;
                                                            break;
                                                        case 2:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.WeekToDate;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.WeekToDate, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                        case 3:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.MonthToDate;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.MonthToDate, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                        case 4:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.YearToDate;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.YearToDate, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                    }
                                                    objSMSSection.smsPeriod = lstSMSPeriod;
                                                }
                                                lstSMSSection.Add(objSMSSection);
                                                objSMSSection = new SMSSection();
                                            }

                                            #endregion
                                            break;
                                        case (int)Enumerations.SMSType.PartyCount:
                                            #region -- Party Count --

                                            string[] PartyCount = lstJobSMS[j].SMSPeriod.Split(';');
                                            if (PartyCount.Length != 0)
                                            {
                                                List<SMSPeriod> lstSMSPeriod = new List<SMSPeriod>();
                                                objSMSSection.smsType = (int)Enumerations.SMSType.PartyCount;
                                                int i = 0;
                                                foreach (string id in PartyCount)
                                                {
                                                    switch (i)
                                                    {
                                                        case 0:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.CurrentDay;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.CurrentDay, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                        case 1:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.Yesterday;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.Yesterday, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }

                                                            i++;
                                                            break;
                                                        case 2:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.WeekToDate;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.WeekToDate, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                        case 3:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.MonthToDate;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.MonthToDate, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                        case 4:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.YearToDate;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.YearToDate, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                    }
                                                    objSMSSection.smsPeriod = lstSMSPeriod;
                                                }
                                                lstSMSSection.Add(objSMSSection);
                                                objSMSSection = new SMSSection();
                                            }

                                            #endregion
                                            break;
                                        case (int)Enumerations.SMSType.CheckAverage:
                                            #region -- Check Average --

                                            string[] CheckAverage = lstJobSMS[j].SMSPeriod.Split(';');
                                            if (CheckAverage.Length != 0)
                                            {
                                                List<SMSPeriod> lstSMSPeriod = new List<SMSPeriod>();
                                                objSMSSection.smsType = (int)Enumerations.SMSType.CheckAverage;
                                                int i = 0;
                                                foreach (string id in CheckAverage)
                                                {
                                                    switch (i)
                                                    {
                                                        case 0:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.CurrentDay;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.CurrentDay, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                        case 1:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.Yesterday;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.Yesterday, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }

                                                            i++;
                                                            break;
                                                        case 2:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.WeekToDate;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.WeekToDate, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                        case 3:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.MonthToDate;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.MonthToDate, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                        case 4:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.YearToDate;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.YearToDate, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                    }
                                                    objSMSSection.smsPeriod = lstSMSPeriod;
                                                }
                                                lstSMSSection.Add(objSMSSection);
                                                objSMSSection = new SMSSection();
                                            }

                                            #endregion
                                            break;
                                        case (int)Enumerations.SMSType.LaborCosts:
                                            #region -- Labor Costs --

                                            string[] LaborCosts = lstJobSMS[j].SMSPeriod.Split(';');
                                            if (LaborCosts.Length != 0)
                                            {
                                                List<SMSPeriod> lstSMSPeriod = new List<SMSPeriod>();
                                                objSMSSection.smsType = (int)Enumerations.SMSType.LaborCosts;
                                                int i = 0;
                                                foreach (string id in LaborCosts)
                                                {
                                                    switch (i)
                                                    {
                                                        case 0:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.CurrentDay;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.CurrentDay, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }

                                                            i++;
                                                            break;
                                                        case 1:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.Yesterday;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.Yesterday, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }

                                                            i++;
                                                            break;
                                                        case 2:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.WeekToDate;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.WeekToDate, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                        case 3:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.MonthToDate;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.MonthToDate, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                        case 4:
                                                            if (Convert.ToInt32(id) != 0)
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                obj.smsPeriod = (int)Enumerations.SMSPeriod.YearToDate;
                                                                obj.fromdate = Convert.ToString(SRDMAPIHelper.GetSMSStartDateTime((int)Enumerations.SMSPeriod.YearToDate, objJob.TimeZone, out endDate));
                                                                obj.todate = Convert.ToString(endDate.ToShortDateString());
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            else
                                                            {
                                                                SMSPeriod obj = new SMSPeriod();
                                                                lstSMSPeriod.Add(obj);
                                                            }
                                                            i++;
                                                            break;
                                                    }
                                                    objSMSSection.smsPeriod = lstSMSPeriod;
                                                }
                                                lstSMSSection.Add(objSMSSection);
                                                //objSMSSection = new SMSSection();
                                            }

                                            #endregion
                                            break;
                                    }
                                }

                                for (int i = 0; i < lstSMSSection.Count; i++)
                                {
                                    List<KeyValue> property = new List<KeyValue>();
                                    List<KeyValue> profitCenter = new List<KeyValue>();

                                    if (lstJobSMS[i].ReportSection == (int)Enumerations.ReportSection.System)
                                    {
                                        reportParameters.level = 2;
                                    }
                                    if (lstJobSMS[i].ReportSection == (int)Enumerations.ReportSection.Property)
                                    {
                                        int rid = lstJobSMS[i].id;
                                        objJobSection = db.JobSections.Where(x => x.ReportId == rid).ToList(); //&& x.sectionType == objJob.ReportSection
                                        foreach (JobSection jobSection in objJobSection)
                                        {
                                            KeyValue P = new KeyValue();
                                            P.Id = jobSection.SectionId;
                                            reportParameters.level = 1;
                                            reportParameters.locationId = P.Id;
                                            property.Add(P);
                                        }
                                    }
                                    if (lstJobSMS[i].ReportSection == (int)Enumerations.ReportSection.ProfitCenter)
                                    {
                                        int rid = lstJobSMS[i].id;

                                        JobSM objJobSMS = db.JobSMS.Where(x => x.id == rid).SingleOrDefault();

                                        if (objJobSMS != null)
                                            reportParameters.PropertyId = (Guid)objJobSMS.PropertyId;

                                        objJobSection = db.JobSections.Where(x => x.ReportId == rid).ToList(); //&& x.sectionType == objJob.ReportSectiontion
                                        foreach (JobSection jobSection in objJobSection)
                                        {
                                            KeyValue PC = new KeyValue();
                                            PC.Id = jobSection.SectionId;
                                            reportParameters.level = 0;
                                            reportParameters.locationId = PC.Id;

                                            profitCenter.Add(PC);
                                        }
                                    }

                                    reportParameters.ProfitCenters = profitCenter;
                                    reportParameters.properties = property;

                                    reportParameters.ReportType = lstJobSMS[i].ReportSection;

                                    reportParameters.SMSType = lstSMSSection[i].smsType;
                                    reportParameters.smsSection = lstSMSSection[i];

                                    ServiceResponse result = new ServiceResponse();
                                    result = System.Threading.Tasks.Task.Run(() => SRDMAPIHelper.ExecuteReport(reportParameters, user.WebAddress)).Result;

                                    if (result.Status == (int)SRDM_BLL.Enumeration.Enumerations.Status.Success)
                                    {
                                        try
                                        {
                                            string response = string.Empty;
                                            string smsEmail = string.Empty;
                                            string phoneNumber = string.Empty;
                                            string smsMsg = string.Concat("SkyWire SRDM \r\n", result.Result.ToString());

                                            List<string> lstEmail = new List<string>();
                                            List<string> lstPhoneNumber = new List<string>();

                                            string[] lstLists = objDestination.DestinationPath.Split(';');

                                            for (int p = 0; p < lstLists.Length; p++)
                                            {
                                                if (lstLists[p].Contains("@"))
                                                {
                                                    lstEmail.Add(lstLists[p]);
                                                }
                                                else
                                                {
                                                    lstPhoneNumber.Add(lstLists[p]);
                                                }
                                            }

                                            if (lstEmail.Count > 0)
                                            {
                                                smsEmail = string.Join(";", lstEmail);
                                            }

                                            if (lstPhoneNumber.Count > 0)
                                            {
                                                phoneNumber = string.Join(";", lstPhoneNumber);
                                            }


                                            if (!string.IsNullOrEmpty(smsEmail))
                                            {
                                               string msg = smsMsg.Replace("\r\n", "<br/>");
                                                ServiceResponse smsresult = new ServiceResponse();
                                                smsresult = System.Threading.Tasks.Task.Run(() => SRDMAPIHelper.SendSMSonEmail(smsEmail, "SkyWire SRDM SMS Report", msg, user.WebAddress)).Result;
                                                response = Convert.ToString(smsresult.Message);
                                            }
                                            if (!string.IsNullOrEmpty(phoneNumber))
                                            {
                                                if (phoneNumber.Contains(';'))
                                                    response = SubmitBulkSMS(smsMsg, phoneNumber.Replace("-", "").Trim(), user.id);
                                                else
                                                    response = SubmitMessage(smsMsg, phoneNumber.Replace("-", "").Trim(), user.id);
                                            }

                                            #region -- Log --
                                            actionInfo = new SRDM_BLL.Log4Net.ActionLoggerInfo()
                                            {
                                                Module = Enumerations.GetDescription(Enumerations.Modules.JobExecution),
                                                DateTime = DateTime.Now,
                                                Status = (int)SRDM_BLL.Enumeration.Enumerations.Status.Success,
                                                DestinationType = Enumerations.GetDescription((SRDM_BLL.Enumeration.Enumerations.DestinationType)objDestination.DestinationType),
                                                CompanyName = user.Company,
                                                Message = string.Concat("SMS job sent successfully for destination :\n - Destination ID: ", objDestination.id, "\n - Job ID: ",
                                                      objJobs.id, "\n - Origination: ", reportParameters.Company, "\n - Destination Name: ", reportParameters.DestinationName,
                                                      "\n - Job Destination: ", objDestination.DestinationPath, "\n \n Job Parameters",
                                                      "\n - Time Period: ", reportParameters.TimePeriod, "\n - Time Zone: ", reportParameters.TimeZone, "\n - IP Address: ", GetLocalIPAddress(),
                                                      "\n - SMS Period: ", Enumerations.GetDescription((SRDM_BLL.Enumeration.Enumerations.SMSType)lstSMSSection[i].smsType),
                                                      "\n - Message: ", response),
                                                //  "\n - SMS:\n ******************************************* \n", smsMsg, "\n *******************************************"),
                                                UserId = user.id,
                                                WebAddress = user.WebAddress,
                                                IPAddress = SRDMAPIHelper.GetLocalIPAddress()
                                            };
                                            logger.LogError(actionInfo);
                                            #endregion
                                        }
                                        catch (Exception ex)
                                        {
                                            #region -- Log --
                                            actionInfo = new SRDM_BLL.Log4Net.ActionLoggerInfo()
                                            {
                                                Module = Enumerations.GetDescription(Enumerations.Modules.JobExecution),
                                                DateTime = DateTime.Now,
                                                Status = (int)SRDM_BLL.Enumeration.Enumerations.Status.Failure,
                                                DestinationType = Enumerations.GetDescription((SRDM_BLL.Enumeration.Enumerations.DestinationType)objDestination.DestinationType),
                                                CompanyName = user.Company,
                                                Message = string.Concat("Error occurred while sending job For destination : \n - Destination ID: ", objDestination.id, "\n - Job ID: ",
                                                    objJobs.id, "\n - Origination: ", reportParameters.Company, "\n - Destination Name: ", reportParameters.DestinationName,
                                                    "\n - Job Destination: ", objDestination.DestinationPath, "\n \n Job Parameters",
                                                    "\n - Time Period: ", reportParameters.TimePeriod, "\n - Time Zone: ", reportParameters.TimeZone, "\n - IP Address: ", GetLocalIPAddress(),
                                                    "\n - SMS Period: ", Enumerations.GetDescription((SRDM_BLL.Enumeration.Enumerations.SMSType)lstJobSMS[i].SMSType),
                                                    "\n - Error Message: ", ex.Message),
                                                UserId = user.id,
                                                WebAddress = user.WebAddress,
                                                IPAddress = SRDMAPIHelper.GetLocalIPAddress()
                                            };
                                            logger.LogError(actionInfo);

                                            #endregion
                                        }
                                    }
                                    else if (result.Status == (int)SRDM_BLL.Enumeration.Enumerations.Status.Failure)
                                    {
                                        #region -- Log --
                                        actionInfo = new SRDM_BLL.Log4Net.ActionLoggerInfo()
                                        {
                                            Module = Enumerations.GetDescription(Enumerations.Modules.JobExecution),
                                            DateTime = DateTime.Now,
                                            Status = (int)SRDM_BLL.Enumeration.Enumerations.Status.Failure,
                                            DestinationType = Enumerations.GetDescription((SRDM_BLL.Enumeration.Enumerations.DestinationType)objDestination.DestinationType),
                                            CompanyName = user.Company,
                                            Message = string.Concat("Error occurred while sending job For destination : \n - Destination ID: ", objDestination.id, "\n - Job ID: ",
                                                objJobs.id, "\n - Origination: ", reportParameters.Company, "\n - Destination Name: ", reportParameters.DestinationName,
                                                "\n - Job Destination: ", objDestination.DestinationPath, "\n \n Job Parameters",
                                                "\n - Time Period: ", reportParameters.TimePeriod, "\n - Time Zone: ", reportParameters.TimeZone, "\n - IP Address: ", GetLocalIPAddress(),
                                                "\n - SMS Period: ", Enumerations.GetDescription((SRDM_BLL.Enumeration.Enumerations.SMSType)lstJobSMS[i].SMSType),
                                                "\n - Error Message: ", result.Message),
                                            UserId = user.id,
                                            WebAddress = user.WebAddress,
                                            IPAddress = SRDMAPIHelper.GetLocalIPAddress()
                                        };
                                        logger.LogError(actionInfo);

                                        #endregion
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                #region -- Log --
                var actionInfo = new SRDM_BLL.Log4Net.ActionLoggerInfo()
                {
                    Module = Enumerations.GetDescription(Enumerations.Modules.JobExecution),
                    DateTime = DateTime.Now,
                    Status = (int)SRDM_BLL.Enumeration.Enumerations.Status.Failure,
                    DestinationType = Enumerations.GetDescription((SRDM_BLL.Enumeration.Enumerations.DestinationType)objDestination.DestinationType),
                    CompanyName = user.Company,
                    Message = string.Concat("Error occurred while job excution For destination : \n - Destination ID: ", objDestination.id, "\n - Job ID: ",
                                            objJobs.id, "\n - Origination: ", reportParameters.Company, "\n - Destination Name: ", reportParameters.DestinationName, "\n \n Job Parameters",
                                            "\n - Time Period: ", reportParameters.TimePeriod, "\n - Time Zone: ", reportParameters.TimeZone, "\n - IP Address: ", GetLocalIPAddress(),
                                            "\n - Report name: ", reportParameters.ReportName, "\n - Error Message: ", ex.Message),
                    UserId = user.id,
                    WebAddress = user.WebAddress,
                    IPAddress = SRDMAPIHelper.GetLocalIPAddress()
                };
                logger.LogError(actionInfo);
                #endregion
            }
        }

        /// <summary>
        /// Send SMS 
        /// </summary>
        /// <param name="emailTo"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="strSRDMApiURL"></param>
        /// <returns></returns>
        private static async Task<ServiceResponse> SendSMSonEmail(string emailTo, string subject, string message, string strSRDMApiURL)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    ServiceResponse objResponse = new ServiceResponse();
                    HttpClient client = new HttpClient();

                    // Create the JSON formatter.
                    MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();

                    SMSEmail smsParameters = new SMSEmail();
                    smsParameters.EmailTo = emailTo;
                    smsParameters.EmailSubject = subject;
                    smsParameters.MessageBody = message;

                    // Use the JSON formatter to create the content of the request body.
                    HttpContent content = new ObjectContent<SMSEmail>(smsParameters, jsonFormatter);

                    // Send the request.
                    HttpResponseMessage response = await client.PostAsync(string.Concat(strSRDMApiURL, urlSendEmail), content);


                    if (response.IsSuccessStatusCode)
                    {
                        objResponse = response.Content.ReadAsAsync<ServiceResponse>().Result;
                    }


                    objResponse.Message = response.ReasonPhrase + " : " + response.StatusCode + " : " + response.Content.ReadAsStringAsync().Result;

                    return objResponse;
                }
                catch (Exception)
                {
                    return new ServiceResponse();
                }

            }
        }

        /// <summary>
        /// Check is EmailAddress
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private static bool IsEmailAddress(string email)
        {
            /*Regular Expressions for email id*/
            string pattern = null;
            pattern = @"^(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+([;.](([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+)*$";
            if ((Regex.IsMatch(email, pattern)))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Logs the test error.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public static void logTestError(string msg)
        {
            using (StreamWriter w = File.AppendText(@"D:\\Logs\\Job.txt"))
            {
                w.Write("\r\nLog Entry : ");
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                w.WriteLine(msg);
                w.WriteLine("-------------------------------");
            }
        }

        /// <summary>
        /// Excutes the database.
        /// </summary>
        public static void ExcuteDatabase()
        {
            try
            {
                string databaseName = ConfigurationManager.AppSettings["DatabaseName"].ToString();
                string applicationVersion = GetAssemblyVersion(Assembly.GetExecutingAssembly());
                SRDMEntities db = new SRDMEntities();
                bool result = false;
                string innerConnectionString = string.Empty;
                innerConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SRDMEntities"].ConnectionString;
                string providerConnectionString = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(innerConnectionString).ProviderConnectionString;
                string blankSqlConnectionString = GetBlankConnection();
                //string blanckSqlConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BlankDBConnection"].ConnectionString;

                SqlConnection conn = new SqlConnection(blankSqlConnectionString);
                result = SRDMAPIHelper.CheckDatabaseExists(conn, databaseName);

                if (!result)
                {
                    using (var connection = new SqlConnection(blankSqlConnectionString))
                    {
                        string createDB = "CREATE DATABASE " + databaseName;

                        using (SqlCommand cmd = new SqlCommand(createDB, connection))
                        {
                            connection.Open();
                            cmd.ExecuteNonQuery();
                            connection.Close();
                        }

                        try
                        {
                            string loginSysAdmin = "If not Exists (select loginname from master.dbo.syslogins  where name = '[NT AUTHORITY\\SYSTEM]' and dbname = '" + databaseName + "')" +
                                                    "Begin EXEC sp_addsrvrolemember 'NT AUTHORITY\\SYSTEM', 'sysadmin'; End";
                            using (SqlCommand cmd = new SqlCommand(loginSysAdmin, connection))
                            {
                                connection.Open();
                                cmd.ExecuteNonQuery();
                                connection.Close();
                            }
                        }
                        catch (Exception ex) { LogError(ex.Message); }

                        using (SqlCommand cmd = connection.CreateCommand())
                        {
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.CommandTimeout = 0;//unlimited
                            var regex = new Regex(Environment.NewLine + "go", RegexOptions.IgnoreCase);
                            string content = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\SRDM_2.0.sql");
                            //remove go statement from script because it is not tsql statement.
                            content = regex.Replace(content, string.Empty);
                            cmd.CommandText = content;
                            if (connection.State == System.Data.ConnectionState.Closed)
                                connection.Open();
                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (SqlException)
                            {
                                return;
                            }
                        }
                    }
                    var versionno = GetApplicationVersion(); // (from v in db.Versions select v.versionno).FirstOrDefault();
                    if (versionno != applicationVersion)
                        ExecuteScript(providerConnectionString, AppDomain.CurrentDomain.BaseDirectory + "\\Scripts", "*.sql", "2.0", applicationVersion);
                }
                else
                {
                    var versionno = GetApplicationVersion();// (from v in db.Versions select v.versionno).FirstOrDefault();
                    if (versionno != applicationVersion)
                        ExecuteScript(providerConnectionString, AppDomain.CurrentDomain.BaseDirectory + "\\Scripts", "*.sql", versionno.ToString(), applicationVersion);
                }
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Checks the database exists.
        /// </summary>
        /// <param name="tmpConn">The temporary connection.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool CheckDatabaseExists(SqlConnection tmpConn, string databaseName)
        {
            string sqlCreateDBQuery;
            bool result = false;
            try
            {
                sqlCreateDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", databaseName);
                using (tmpConn)
                {
                    using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn))
                    {
                        tmpConn.Open();
                        object resultObj = sqlCmd.ExecuteScalar();
                        int databaseID = 0;
                        if (resultObj != null)
                        {
                            int.TryParse(resultObj.ToString(), out databaseID);
                        }
                        tmpConn.Close();
                        result = (databaseID > 0);
                    }
                }
            }
            catch (Exception)
            {
                tmpConn.Close();
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Executes the script.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="directory">The directory.</param>
        /// <param name="scriptFileExtension">The script file extension.</param>
        /// <param name="version">The version.</param>
        /// <param name="applicationVersion">The application version.</param>
        public static void ExecuteScript(string connectionString, string directory, string scriptFileExtension, string version, string applicationVersion)
        {
            int fromId = 0;
            //sort filenames
            string[] files = Directory.GetFiles(directory, scriptFileExtension, SearchOption.AllDirectories).OrderBy(f => f).ToArray();
            string curentFilename = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\Scripts\\SRDM_", version, ".sql");

            List<KeyValuePair<string, int>> fileList = new List<KeyValuePair<string, int>>();
            int i = 0;
            foreach (var item in files)
            {
                string[] versionArray = item.Split('_');
                string ver = versionArray[1].Replace(".sql", "");

                if (Convert.ToDouble(version) < Convert.ToDouble(ver))
                {
                    fileList.Add(new KeyValuePair<string, int>(item, i));
                    i++;
                }
            }

            fromId = fileList.Where(a => a.Key == curentFilename).Select(a => a.Value).FirstOrDefault();

            if (null == files || files.Length == 0)
                return;

            // var result = true;
            using (var conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandTimeout = 0;//unlimited
                    var regex = new Regex(Environment.NewLine + "go", RegexOptions.IgnoreCase);
                    //execute from current version
                    if (fileList.Count > 0)
                    {
                        for (int fileIndex = 0; fileIndex < fileList.Count; fileIndex++)
                        {
                            if (File.Exists(fileList[fileIndex].Key))
                            {
                                string content = File.ReadAllText(fileList[fileIndex].Key);
                                //remove go statement from script because it is not tsql statement.
                                content = regex.Replace(content, string.Empty);
                                cmd.CommandText = content;
                                if (conn.State == System.Data.ConnectionState.Closed)
                                    conn.Open();
                                try
                                {
                                    cmd.ExecuteNonQuery();
                                    conn.Close();
                                }
                                catch (SqlException)
                                {
                                    conn.Close();
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            //update current version in database
            try
            {
                var versionno = GetApplicationVersion();
                UpdateVersion(applicationVersion);
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Tests the connection.
        /// </summary>
        /// <param name="blanckSqlConnectionString">The blanck SQL connection string.</param>
        /// <returns><c>true</c>  <c>false</c> otherwise.</returns>
        public static bool TestConnection(string blanckSqlConnectionString)
        {
            bool result = false;
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(blanckSqlConnectionString))
            {
                try
                {
                    connection.Open();
                    result = true;
                    connection.Close();
                    return result;
                }
                catch (System.Data.SqlClient.SqlException)
                {
                    result = false;
                    return result;
                }
            }
        }

        /// <summary>
        /// Gets the blank connection.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetBlankConnection()
        {
            string blankSqlConnectionString = "";
            string innerConnectionString = string.Empty;
            innerConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SRDMEntities"].ConnectionString;
            string providerConnectionString = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(innerConnectionString).ProviderConnectionString;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(providerConnectionString);
            blankSqlConnectionString = "data source=" + builder.DataSource + ";user id=" + builder.UserID + ";password=" + builder.Password + ";";
            return blankSqlConnectionString;
        }

        /// <summary>
        /// Updates the service.
        /// </summary>
        /// <param name="action">The action.</param>
        public static void UpdateService(string action)
        {
            Process proc = new Process();
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.FileName = "net.exe";
            proc.StartInfo.Arguments = String.Format("{0} \"SkyWire SRDM\"", action);
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            if (proc.HasExited == false)
            {
                proc.WaitForExit(10000);
            }

            proc.EnableRaisingEvents = true;

            proc.Close();

        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private static void LogError(string msg)
        {
            var actionInfo = new SRDM_BLL.Log4Net.ActionLoggerInfo()
            {
                Module = Enumerations.GetDescription(Enumerations.Modules.JobExecution),
                DateTime = DateTime.Now,
                Status = (int)SRDM_BLL.Enumeration.Enumerations.Status.Failure,
                DestinationType = "",
                CompanyName = "",
                Message = msg,
                UserId = 0,
                WebAddress = "",
                IPAddress = SRDMAPIHelper.GetLocalIPAddress()
            };
            logger.LogError(actionInfo);
        }

        /// <summary>
        /// Submits the message.
        /// </summary>
        /// <param name="messageText">The message text.</param>
        /// <param name="cellno">The cellno.</param>
        /// <param name="uID">The u identifier.</param>
        /// <returns>System.String.</returns>
        public static string SubmitMessage(string messageText, string cellno, int uID)
        {
            SMSConfig objSMSConfig = new SMSConfig();
            objSMSConfig = GetSMSConfigDetailsForUser(uID);
            skylink oSkyLink = new skylink();
            SubmitSMSRequest req = new SubmitSMSRequest()
            {
                property_id = Convert.ToInt32(objSMSConfig.PropertyID ?? "0"), //Demo Property ID
                username = objSMSConfig.APIUserName, //Demo Username
                seckey = objSMSConfig.APIPassword, //Demo Password
                //messageType = "SKYLINK-DEMO", //String that uniquely identifies the type of message
                destinationAddr = cellno, //Cell number formatted as 11 numeric digits, including a leading 1.
                destinationOperator = 0, //Numeric carrier ID (if known), 0 if not known.
                messageText = messageText //Message text to send. Must be 160 characters or less, and not include any double-byte character values (English only)
            };
            SubmitSMSResponse resp = oSkyLink.SubmitSMS(req);
            return resp.response;
        }

        /// <summary>
        /// Gets the SMS configuration details for user.
        /// </summary>
        /// <param name="uID">The uID.</param>
        /// <returns>SMSConfig.</returns>
        public static SMSConfig GetSMSConfigDetailsForUser(int uID)
        {
            SRDMEntities db = new SRDMEntities();
            SMSConfig objSMSConfig = new SMSConfig();
            objSMSConfig = db.SMSConfigs.Where(s => s.UserID == uID).FirstOrDefault();
            return objSMSConfig;
        }

        public static string SubmitBulkSMS(string message, string cellNumberList, int uID)
        {
            SMSConfig objSMSConfig = new SMSConfig();
            objSMSConfig = GetSMSConfigDetailsForUser(uID);

            //if (cellNumberList.Count == 0) return false;
            //if (cellNumberList.Count == 1) return SubmitMessage(message,cellNumberList[0], uID) ;

            var phoneString = cellNumberList; ///cellNumberList.Aggregate("", (current, number) => current + (number + ";"));
            skylink oSkyLink = new skylink();
            var request = new SubmitBulkSMSRequest
            {
                username = objSMSConfig.APIUserName,
                seckey = objSMSConfig.APIPassword,
                messageText = message,
                destinationAddr = phoneString,
                property_id = Convert.ToInt32(objSMSConfig.PropertyID ?? "0"),
            };
            SubmitBulkSMSResponse response = oSkyLink.SubmitBulkSMS(request);

            //if (string.IsNullOrEmpty(response.response)) return false;
            var lowerResponse = response.response.ToLower();

            //if (lowerResponse.Contains("success") || lowerResponse.Contains("correct")) return true;
            return lowerResponse;
        }
    }

    /// <summary>
    /// Printers class
    /// </summary>
    public static class myPrinters
    {
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);

    }
}
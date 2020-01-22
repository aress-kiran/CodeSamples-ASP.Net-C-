using System.Collections.Generic;
using System.Web.Services;
using System.Collections;
using System.Data;
using System;

/// <summary>
/// Summary description for WsEmployeePosition
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WsEmployeePosition : System.Web.Services.WebService
{

    #region " Variable Declarations "
    EmployeePosition oEmployeePosition = new EmployeePosition();
    #endregion

    #region " Constructor "
    /// <summary>
    /// Initializes a new instance of the <see cref="WsEmployeePosition"/> class.
    /// </summary>
    public WsEmployeePosition()
    {
        InitializeComponent();
    }
    #endregion

    #region " Web Methods "
    /// <summary>
    /// Gets the employee positions.
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public List<EmployeePositionData> GetEmployeePositions()
    {
        return oEmployeePosition.GetEmployeePositions();
    }

    /// <summary>
    /// Deletes the employee position.
    /// </summary>
    /// <param name="positionID">The position identifier.</param>
    /// <returns></returns>
    [WebMethod]
    public int DeleteEmployeePosition(int positionID)
    {
        return oEmployeePosition.DeleteEmployeePosition(positionID);
    }

    /// <summary>
    /// Gets the employee position.
    /// </summary>
    /// <param name="positionID">The position identifier.</param>
    /// <returns></returns>
    [WebMethod]
    public EmployeePositionData GetEmployeePosition(int positionID)
    {
        return oEmployeePosition.GetEmployeePosition(positionID);
    }

    /// <summary>
    /// Gets the curriculum by position.
    /// </summary>
    /// <param name="positionID">The position identifier.</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet GetCurriculumByPosition(int positionID)
    {
        return oEmployeePosition.GetCurriculumByPosition(positionID);
    }

    /// <summary>
    /// Gets the training by curriculum.
    /// </summary>
    /// <param name="curriculumID">The curriculum identifier.</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet GetTrainingByCurriculum(int curriculumID)
    {
        return oEmployeePosition.GetTrainingByCurriculum(curriculumID);
    }

    /// <summary>
    /// Gets the employee position identifier.
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public Hashtable GetEmployeePositionID()
    {
        return oEmployeePosition.GetEmployeePositionID();
    }

    /// <summary>
    /// Saves the employee postion.
    /// </summary>
    /// <param name="oEmpPosition">The o emp position.</param>
    /// <param name="userID">The user identifier.</param>
    /// <returns></returns>
    [WebMethod]
    public int SaveEmployeePostion(EmployeePositionData oEmpPosition, int userID)
    {
        return oEmployeePosition.SaveEmployeePosition(oEmpPosition, userID);
    }

    /// <summary>
    /// Saves the curriculum.
    /// </summary>
    /// <param name="PositionID">The position identifier.</param>
    /// <param name="curriculumIds">The curriculum ids.</param>
    /// <returns></returns>
    [WebMethod]
    public int SaveCurriculum(int PositionID, string curriculumIds)
    {
        return oEmployeePosition.SaveCurriculum(PositionID, curriculumIds);
    }

    /// <summary>
    /// Gets the house keeping.
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public List<HouseKeepData> GetHouseKeeping()
    {
        return oEmployeePosition.GetHouseKeeping();
    }

    /// <summary>
    /// Gets the positions to publish.
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public List<EmployeePositionData> GetPositionsToPublish()
    {
        return oEmployeePosition.GetPositionsToPublish();
    }

    /// <summary>
    /// Publishes the employee positions.
    /// </summary>
    /// <param name="positionIDs">The position i ds.</param>
    /// <param name="selectedAppToPublish">The selected application to publish.</param>
    /// <param name="lastUserID">The last user identifier.</param>
    /// <returns></returns>
    [WebMethod]
    public string PublishEmployeePositions(string positionIDs, int lastUserID, string transactionDate)
    {
        return oEmployeePosition.PublishEmployeePositions(positionIDs, lastUserID, transactionDate);
    }

    /// <summary>
    /// Gets the employees to migrate.
    /// </summary>
    /// <param name="clientId">The client identifier.</param>
    /// <param name="facility">The facility.</param>
    /// <returns></returns>
    [WebMethod]
    public List<EmployeePositionData> GetEmployeesToMigrate(int clientId, int facility)
    {
        return oEmployeePosition.GetEmployeesToMigrate(clientId, facility);
    }

    /// <summary>
    /// Copies the employee position.
    /// </summary>
    /// <param name="positionIdToCopy">The position identifier to copy.</param>
    /// <param name="newPositionName">New name of the position.</param>
    /// <param name="lastUserId">The last user identifier.</param>
    /// <returns></returns>
    [WebMethod]
    public int CopyEmployeePosition(int positionIdToCopy, string newPositionName, int lastUserId)
    {
        return oEmployeePosition.CopyEmployeePosition(positionIdToCopy, newPositionName, lastUserId);
    }

    /// <summary>
    /// Transfers the employee trainings.
    /// </summary>
    /// <param name="sourceClientId">The source client identifier.</param>
    /// <param name="SourceFacilityId">The source facility identifier.</param>
    /// <param name="destinationClientId">The destination client identifier.</param>
    /// <param name="destinationFacilityId">The destination facility identifier.</param>
    /// <param name="sourceEmployeeId">The source employee identifier.</param>
    /// <param name="action">The action.</param>
    /// <param name="newSupervisorId">The new supervisor identifier.</param>
    /// <param name="isTransferResp">The is transfer resp.</param>
    /// <param name="openDataSourceQuery">The open data source query.</param>
    /// <param name="loginUserId">The login user identifier.</param>
    /// <param name="transactionDate">The transaction date.</param>
    /// <returns></returns>
    [WebMethod]
    public int TransferEmployeesTrainings(int sourceClientId, int SourceFacilityId, int destinationClientId, int destinationFacilityId, int sourceEmployeeId, string action, int newSupervisorId, int isTransferResp, string openDataSourceQuery, int loginUserId, string transactionDate)
    {
        return oEmployeePosition.TransferEmployeesTrainings(sourceClientId, SourceFacilityId, destinationClientId, destinationFacilityId, sourceEmployeeId, action, newSupervisorId, isTransferResp, openDataSourceQuery, loginUserId, transactionDate);
    }

    /// <summary>
    /// Checks the is supervisor with open history.
    /// </summary>
    /// <param name="sourceClientId">The source client identifier.</param>
    /// <param name="destinationClientId">The destination client identifier.</param>
    /// <param name="employeeKey">The employee key.</param>
    /// <returns></returns>
    [WebMethod]
    public string CheckIsSupervisorWithOpenHistory(int sourceClientId, int destinationClientId, int employeeKey)
    {
        return oEmployeePosition.CheckIsSupervisorWithOpenHistory(sourceClientId, destinationClientId, employeeKey);
    }

    /// <summary>
    /// Gets all supervisors.
    /// </summary>
    /// <param name="clientId">The client identifier.</param>
    /// <param name="facility">The facility.</param>
    /// <param name="employee">The employee.</param>
    /// <returns></returns>
    [WebMethod]
    public List<EmployeePositionData> GetAllSupervisors(int clientId, int facility, int employee)
    {
        return oEmployeePosition.GetAllSupervisors(clientId, facility, employee);
    }

    /// <summary>
    /// Gets the facility master user.
    /// </summary>
    /// <param name="clientId">The client identifier.</param>
    /// <param name="facility">The facility.</param>
    /// <returns></returns>
    [WebMethod]
    public List<EmployeePositionData> GetFacilityMasterUser(int clientId, int facility)
    {
        return oEmployeePosition.GetFacilityMasterUser(clientId, facility);
    }

    /// <summary>
    /// Gets the positions based on access.
    /// </summary>
    /// <param name="accessType">Type of the access.</param>
    /// <returns></returns>
    [WebMethod]
    public List<EmployeePositionData> GetPositionsBasedOnAccess(int accessType)
    {
        return oEmployeePosition.GetPositionsBasedOnAccess(accessType);
    }

    /// <summary>
    /// Get Position By Curriculum
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public List<EmployeePositionData> GetPositionByCurriculum(Int32 CurriculumId)
    {
        return oEmployeePosition.GetPositionByCurriculum(CurriculumId);
    }
    #endregion
}

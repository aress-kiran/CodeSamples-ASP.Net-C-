using System;


#region " Employee Position Class "
/// <summary>
/// Summary description for EmployeePositionData
/// </summary>
public class EmployeePositionData
{
	#region " Constructor "
	/// <summary>
	/// Initializes a new instance of the <see cref="EmployeePositionData"/> class.
	/// </summary>
	public EmployeePositionData(int positionId, string position)
    {
        PositionID = positionId;
        Position = position;
    }
	#endregion

	#region " Public Properties "
	public int PositionID { get; set; }
	public string Position { get; set; }
	public string Description { get; set; }
	public int HouseKeepingID { get; set; }
	public string HouseKeeper { get; set; }
	public string Employee { get; set; }
	public int EmployeeID { get; set; }
	public bool IsDistributed { get; set; }
	public int ServiceID { get; set; }
	public int ClassificationID { get; set; }
	public string Service { get; set; }
	public string Classification { get; set; }
	public bool ManagementTraining { get; set; }
	public bool IsMaster { get; set; }
	public string EmployeeEmailId { get; set; }
	public int Facility { get; set; }
	public bool IsActive { get; set; }
    public string IsAssociatedToUser { get; set; }
    public bool IsAnnualCertification { get; set; }
	#endregion
}
#endregion
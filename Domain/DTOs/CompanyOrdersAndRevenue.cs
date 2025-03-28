namespace Domain.DTOs;

public class CompanyOrdersAndRevenue
{
    public int CompanyId { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
}
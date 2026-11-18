namespace api_floower.Dtos;


public class LoginDto
{
    public string? Login { get; set; }
    public string? Password { get; set; }
}

public class RoleDto
{
    public int IdRole { get; set; }
    public string? NameRole { get; set; }
}

public class StatusDto
{
    public int IdStatus { get; set; }
    public string? NameStatus { get; set; }
}

public class UserDto
{
    public int IdUser { get; set; }
    public string? Fio { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public int? IdRole { get; set; }
    public string? NameRole { get; set; }
}

public class PostavshikDto
{
    public int IdPost { get; set; }
    public string? NamePost { get; set; }
}

public class ProductDto
{
    public int IdProduct { get; set; }
    public string? NameProduct { get; set; }
    public int? IdPost { get; set; }
    public string? NamePost { get; set; }
    public int? Sell { get; set; }
    public int? Kolvo { get; set; }
    public string? Description { get; set; }
}

public class OrderDto
{
    public int IdOrder { get; set; }
    public int? IdUser { get; set; }
    public string? Fio { get; set; }
    public DateTime? DataZakaz { get; set; }
    public int? CodePoly { get; set; }
    public int? IdStatus { get; set; }
    public string? NameStatus { get; set; }
}

public class ProductorderDto
{
    public int IdProductorders { get; set; }
    public int? IdProduct { get; set; }
    public string? NameProduct { get; set; }
    public int? Kolvo { get; set; }
    public int? IdOrder { get; set; }
}

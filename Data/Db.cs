using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyThueSanTheThao.Data;

public static class Db
{
    public static SqlConnection OpenConnection()
    {
        var cn = new SqlConnection(DatabaseConfig.ConnectionString);
        cn.Open();
        return cn;
    }

    public static DataTable Query(string sql, params SqlParameter[] parameters)
    {
        using var cn = OpenConnection();
        using var cmd = new SqlCommand(sql, cn);
        if (parameters.Length > 0) cmd.Parameters.AddRange(parameters);
        using var da = new SqlDataAdapter(cmd);
        var dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    public static object? Scalar(string sql, params SqlParameter[] parameters)
    {
        using var cn = OpenConnection();
        using var cmd = new SqlCommand(sql, cn);
        if (parameters.Length > 0) cmd.Parameters.AddRange(parameters);
        return cmd.ExecuteScalar();
    }

    public static int Execute(string sql, params SqlParameter[] parameters)
    {
        using var cn = OpenConnection();
        using var cmd = new SqlCommand(sql, cn);
        if (parameters.Length > 0) cmd.Parameters.AddRange(parameters);
        return cmd.ExecuteNonQuery();
    }
}

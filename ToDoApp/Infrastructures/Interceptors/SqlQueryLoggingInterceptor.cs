using System.Data.Common;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ToDoApp.Infrastructures.Interceptors;

public class SqlQueryLoggingInterceptor : DbCommandInterceptor
{
    private Stopwatch stopwatch = new Stopwatch();

    // Bắt đầu đo thời gian khi truy vấn bắt đầu chạy
    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        stopwatch.Restart(); // Đảm bảo Stopwatch bắt đầu lại từ 0
        return base.ReaderExecuting(command, eventData, result);
    }

    // Ghi log sau khi truy vấn hoàn thành
    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        stopwatch.Stop(); // Dừng bấm giờ sau khi truy vấn hoàn thành

        var milliseconds = stopwatch.ElapsedMilliseconds;

        if (milliseconds > 200) // Chỉ log nếu truy vấn lâu hơn 1 giây
        {
            using StreamWriter writer = new StreamWriter("/Users/devbaoo/Learn/ToDoApp/ToDoApp/sqllog.txt", true);
            writer.WriteLine($"Query: {command.CommandText} - Time: {milliseconds}ms");
        }

        return base.ReaderExecuted(command, eventData, result);
    }
}
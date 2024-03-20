using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegramchik.Commands.Filters;

public interface IFilters
{
    public long ChatId { get; set; }
    public string Text { get; set; }
    public byte Type { get; set; }
    public string FileId { get; set; }
    public string Name { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsVote.Shared.DTO;
public class GameOverArgs
{
    public string WinningTeam { get; set; } = default!;
    public int TeamA { get; set; }
    public int TeamB { get; set; }
}

using GameOfLife.Data;
using GameOfLife.UI;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            UIBase menu = new MainMenu();
            await menu.Display();
        }
    }
}

using System.Linq;
using Rewired;

namespace Datenshi.Scripts.Util {
    public static class InputUtil {
        public static bool GetAnyPlayerButtonDown(int action) {
            return ReInput.players.Players.Any(player => player.GetButtonDown(action));
        }
    }
}
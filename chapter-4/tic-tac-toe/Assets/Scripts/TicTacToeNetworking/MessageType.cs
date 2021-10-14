public enum MessageType
{
    ServerStartGame,        // Show the game UI (disabled player too)
    ServerTogglePlayer,     // Toggle the ability of the player to interact with the game
    ServerShowPodium,       // Show the podium
    ClientMakeMove,         // Client makes a move
    ClientPlayAgain,        // Client wants to play again
    ClientQuit              // Client quit to the menu
}

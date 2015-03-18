// --------------------------
// RP Money :: User
// --------------------------

if (isPackage(RP_Money_User_Core))
	deactivatePackage(RP_Money_User_Core);

package RP_Money_User_Core
{
	// Player entering a game
	function GameConnection::EnterRPGame(%client)
	{
		Parent::EnterRPGame(%client);
		
		// Check for GUI
		%client.SendModToGUI("Money");
	}
};
activatePackage(RP_Money_User_Core);

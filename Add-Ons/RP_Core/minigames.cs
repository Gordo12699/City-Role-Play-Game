// --------------------------
// RP Core :: MiniGame
// --------------------------

// Create minigame
function CreateMiniGame()
{
	echo("RP Core: Creating MiniGame");
	
	RemoveMiniGame();
	
	// Construct minigame
	$RP::minigame = new scriptObject(RP_MiniGame)
	{
		class = MinigameSO;
		
		// Standards
		brickRespawnTime = 10000;
		colorIdx = -1;
		
		// True/False
		brickDamage = true;
		EnableBuilding = true;
		enablePainting = true;
		enableWand = true;
		fallingDamage = true;
		inviteOnly = false;
		PlayersUseOwnBricks = false;
		selfDamage = true;
		useAllPlayersBricks = true;
		useSpawnBricks = false;
		VehicleDamage = true;
		weaponDamage = true;
		
		// Points
		Points_PlantBrick = 0;
		Points_BreakBrick = 0;
		Points_Die = 0;
		Points_KillPlayer = 1;
		Points_KillSelf = -1;
		
		// Datablock
		playerDataBlock = RP_Player;
		
		// Respawn
		respawnTime = 5000;
		vehicleRespawnTime = 5000;
		
		// Amount members
		numMembers = 0;
	};
	
	// Populate minigame
	for (%i = 0; %i < ClientGroup.getCount(); %i++)
		AddMember(ClientGroup.getObject(%i));
}

// Remove minigame
function RemoveMiniGame()
{
	// Remove connected clients
	for (%i = 0; %i < ClientGroup.getCount(); %i++)
		ClientGroup.getObject(%i).minigame = 0;
	
	// Clear minigame
	if (isObject(RP_MiniGame))
		RP_MiniGame.delete();
}

// Add member to minigame
function AddMember(%client)
{
	if (!isObject(RP_MiniGame))
		CreateMiniGame();
	else
	{
		RP_MiniGame.addMember(%client);
	}
}

// Remove member from minigame
function RemoveMember(%client)
{
	if (isObject(RP_MiniGame))
		RP_MiniGame.removeMember(%client);
}

if (isPackage(RP_Core_Minigame))
	deactivatePackage(RP_Core_Minigame);

package RP_Core_Minigame
{
	// Create minigame
	function ServerCmdCreateMiniGame(%client)
	{
		messageClient(%client, '', "\c3Creating a minigame is disabled when the RP is up.");
	}
	// Leave minigame
	function ServerCmdLeaveMiniGame(%client)
	{
		if (!%client.isSuperAdmin)
			return;
		%client.LeaveRPGame();
	}
	// Reset game for maintence
	function ServerCmdResetMiniGame(%client)
	{
		if (!%client.isSuperAdmin)
			return;
		if (%client.isInRP())
			return;
		
		CreateMiniGame();
	}

	// Make every item accessible from minigame
	function MiniGameCanUse(%obj1, %obj2)
	{
		return 1;
	}

	// Make everything damageble
	function MiniGameCanDamage(%obj1, %obj2)
	{
		if (!isObject($RP::minigame))
			return Parent::MiniGameCanDamage(%obj1, %obj2);
		%client = %obj.client;
		if (isObject(%client) && %client.isInRP() && isObject(%obj2) && %obj2.isInRP())
			return 1;
		return Parent::MiniGameCanDamage(%obj1, %obj2);
	}
};
RPRegPackage(RP_Core_Minigame);

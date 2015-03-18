function CRPG_AssembleRealEstateEvents()
{
	// Basic Input
	registerInputEvent("fxDTSBrick", "onEnterLot", "Self fxDTSBrick" TAB "Player player" TAB "Client gameConnection");
	registerInputEvent("fxDTSBrick", "onLeaveLot", "Self fxDTSBrick" TAB "Player player" TAB "Client gameConnection");
	
	for(%d = 0; %d < ClientGroup.getCount(); %d++)
	{
		%subClient = ClientGroup.getObject(%d);
		serverCmdRequestEventTables(%subClient);
		messageClient(%subClient, '', "\c6Your Event Tables have been updated. If you do not know what that means, ignore this message.");
	}
}

// Input Events
function fxDTSBrick::onEnterLot(%brick, %obj)
{
	$inputTarget_self = %brick;
	
	$inputTarget_client = %obj.client;
	$inputTarget_player = %obj.client.player;
	
	$inputTarget_miniGame = (isObject(getMiniGameFromObject(%obj.client))) ? getMiniGameFromObject(%obj.client) : 0;
	
	%brick.processInputEvent("OnEnterLot", %obj.client);
}

function fxDTSBrick::onLeaveLot(%brick, %obj)
{
	$inputTarget_self = %brick;
	
	$inputTarget_client = %obj.client;
	$inputTarget_player = %obj.client.player;
	
	$inputTarget_miniGame = (isObject(getMiniGameFromObject(%obj.client))) ? getMiniGameFromObject(%obj.client) : 0;
	
	%brick.processInputEvent("OnLeaveLot", %obj.client);
}

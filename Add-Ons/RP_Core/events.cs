// --------------------------
// RP Core :: Events
// --------------------------

// Input
registerInputEvent(fxDTSBrick, "onRPTrue", "Self fxDTSBrick\tPlayer Player\tClient GameConnection\tMiniGame MiniGame");
registerInputEvent(fxDTSBrick, "onRPFalse", "Self fxDTSBrick\tPlayer Player\tClient GameConnection\tMiniGame MiniGame");

// RP True
function fxDTSBrick::onRPTrue(%brick, %client)
{
	$inputTarget_self = %brick;
	$inputTarget_client = %client;
	$inputTarget_player = %client.player;
	%miniGame = getMiniGameFromObject(%client);
	$inputTarget_miniGame = (isObject(%miniGame)) ? %miniGame : 0;
	%brick.processInputEvent("onRPTrue", %client);
}

// RP False
function fxDTSBrick::onRPFalse(%brick, %client)
{
	$inputTarget_self = %brick;
	$inputTarget_client = %client;
	$inputTarget_player = %client.player;
	%miniGame = getMiniGameFromObject(%client);
	$inputTarget_miniGame = (isObject(%miniGame)) ? %miniGame : 0;
	%brick.processInputEvent("onRPFalse", %client);
}

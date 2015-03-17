function servercmdmyhunger(%client)
{
	if(!isObject(%client.player)) 
		return;
	messageClient(%client,'',"hunger: " @ %client.RPData.value["hunger"]);
}

function serverCmdEducationCurrent(%client)
{
	if(!isObject(%client.player)) 
		return;
	messageClient(%client, '', "\c6You currently have\c3" SPC %client.RPData.value["education"] SPC "education\c6.");
}

function serverCmdEducationAccept(%client)
{
	if(!isObject(%client.player)) 
		return;
		
	if(%client.RPData.value["education"] < 9)
        %price = ((%client.RPData.value["education"] + 1) * 225);
    else if(%client.RPData.value["education"] < 14)
	    %price = ((%client.RPData.value["education"] + 1) * 1000);
    else if(%client.RPData.value["education"] < 29)
	    %price = ((%client.RPData.value["education"] + 1) * 5000);
    else if(%client.RPData.value["education"] < 49)
	    %price = ((%client.RPData.value["education"] + 1) * 10000);
		
	if(!%client.RPData.value["student"])
	{
		if(%client.RPData.value["money"] >= %price)
		{
			%client.RPData.value["student"] = %client.RPData.value["education"] + 1;
			%client.RPData.value["money"] -= %price;
				
			messageClient(%client, '', "\c6You are now enrolled.");
			%client.updateInfo();
		}
		else
		{
			messageClient(%client, '', "\c6It costs \c3$" @ %price SPC "\c6to get enrolled. You do not have enough money.");
		}
	}
	else
	{
		messageClient(%client, '', "\c6Don't get too far ahead of yourself. You're already enrolled for this course.");
	}
}

function serverCmdeducation(%client, %do)
{	
	if(!isObject(%client.player))
		return;
		
    if(%client.RPData.value["education"] < 9)
        %price = ((%client.RPData.value["education"] + 1) * 225);
    else if(%client.RPData.value["education"] < 14)
	    %price = ((%client.RPData.value["education"] + 1) * 1000);
    else if(%client.RPData.value["education"] < 29)
	    %price = ((%client.RPData.value["education"] + 1) * 5000);
    else if(%client.RPData.value["education"] < 49)
	    %price = ((%client.RPData.value["education"] + 1) * 10000);
		
	if(%do $= "accept")
	{

		if(!%client.RPData.value["student"])
		{
			if(%client.RPData.value["money"] >= %price)
			{
				%client.RPData.value["student"] = %client.RPData.value["education"] + 1;
				%client.RPData.value["money"] -= %price;
					
				messageClient(%client, '', "\c6You are now enrolled.");
				%client.updateInfo();
			}
			else
			{
				messageClient(%client, '', "\c6It costs \c3$" @ %price SPC "\c6to get enrolled. You do not have enough money.");
			}
		}
		else
		{
			messageClient(%client, '', "\c6Don't get too far ahead of yourself. You're already enrolled for this course.");
		}
	}
	else if(%do $= "level")
	{
		%level = %client.RPData.value["education"];
		messageClient(%client, '', "\c6You have a level \c3" @ %level @ "\c6 education.");
	}
	else
	{
		if(%client.RPData.value["education"] < 6)
		{
			messageClient(%client, '', "\c6It will cost you \c3$" @ %price @ "\c6 to reach your next diploma.");
			messageClient(%client, '', "\c6 - Type \"\c3/education accept\c6\" to accept.");
			messageClient(%client, '', "\c6 - Type \"\c3/education level\c6\" to see your current diploma.");
		}
		else
		{
			%level = %client.RPData.value["education"];
			messageClient(%client, '', "\c6You have a level \c3" @ %level @ "\c6 education, the highest there is to offer.");
		}
	}
}

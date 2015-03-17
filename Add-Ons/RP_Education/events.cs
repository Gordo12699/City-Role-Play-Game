function CRPG_AssembleHungerEvents()
{	
	for(%a = 1; $CRPG::portion[%a] !$= ""; %a++)
	{
		%sellFood_Portions = %sellFood_Portions SPC $CRPG::portion[%a] SPC %a;
	}
	registerOutputEvent("fxDTSBrick", "RPsellFood", "list" @ %sellFood_Portions TAB "string 45 100" TAB "int 1 50 1");
	
	for(%b = 0; %b < ClientGroup.getCount(); %b++)
	{
		%subClient = ClientGroup.getObject(%b);
		serverCmdRequestEventTables(%subClient);
		messageClient(%subClient, '', "\c6Your Event Tables have been updated. If you do not know what that means, ignore this message.");
	}
}

function fxDTSBrick::RPsellFood(%brick, %portion, %food, %markup, %client)
{
	if(isObject(%client.player) && !%client.player.serviceOrigin  && isObject(%brick))
	{
		%client.player.serviceType = "food";
		%client.player.serviceItem = %food;
		%client.player.serviceSize = %portion;
		%client.player.serviceFee = (5 * %portion - mFloor(%portion * 0.75)) +  %markup;
		%client.player.serviceMarkup = %markup;
		%client.player.serviceOrigin = %brick;
		
		messageClient(%client,'','\c6A service is offering to feed you a(n) \c3%2\c6 portion of \c3%3\c6 for \c3$%4\c6.', CityRPG_DetectVowel($CityRPG::portion[%portion]), strreplace($CRPG::portion[%portion], "_", " "), %food, %client.player.serviceFee);
		messageClient(%client,'',"\c6Accept with \c3/yes\c6, decline with \c3/no\c6.");
	}
	else if(%client.player.serviceOrigin && %client.player.serviceOrigin != %brick)
		messageClient(%client, '', "\c6You already have a charge request from another service! Type \c3/no\c6 to reject it.");
}

function gameConnection::RPsellFood(%client, %sellerID, %servingID, %foodName, %price, %profit)
{
	if(%client.RPData.value["money"] >= %price)
	{
		if(%client.RPData.value["hunger"] < 10)
		{
			%portionName = strreplace($CRPG::portion[%servingID], "_", " ");
					
			if($RP::job::sellFood[RPDB.get(%SellerID, "jobID")] || %sellerID.isAdmin)
			{
				messageClient(%client, '', '\c6You consume a(n) \c3%2\c6 serving of \c3%3\c6.', CityRPG_DetectVowel(%portionName), %portionName, %foodName);
				
				%client.RPData.value["hunger"] += %servingID;
				if(%client.RPData.value["hunger"] > 10)
				{
					%client.RPData.value["hunger"] = 10;
				}
				%client.RPData.value["money"] -= %price;
				
				if(%profit)
				{
					if(isObject(%seller = findClientByBL_ID(%sellerID)))
					{
						messageClient(%seller, '', '\c6You just gained \c3$%1\c6 for providing \c3%3\c6 to \c3%2\c6.', %profit, %client.name, %foodName);
						%seller.RPData.value["bank"] += %profit;
					}
				}
				
				%client.player.setScale("1 1 1");
				%client.updateInfo();
				%client.player.serviceOrigin.onTransferSuccess(%client);
			}
			else
				messageClient(%client, '', "\c6This vendor is not licensed to sell food.");
		}
		else
			messageClient(%client, '', "\c6You are too full to even think about buying any more food.");
	}
	else
		messageClient(%client, '', "\c6You don't have enough money to buy this food.");
}

// Bounty Brick

// Brick Data
datablock fxDtsBrickData(BountyBrickData)
{
	brickfile = "./TvRed.blb";
	iconName = "Add-Ons/RP_Core/LcdTV";
	
	category = "CRPG";
	subcategory = "Admin event bricks";
	
	uiName = "Bounty brick";
	
	specialBrickType = "";
	
	isInfoBrick = 1;
	adminOnly = 1;
};

// Brick functions

// Activate data
function BountyBrickData::Activate(%datablock, %client)
{
	messageClient(%client, '', "\c3Bounties");
	messageClient(%client, '', "\c31. \c6Check bounties");
	messageClient(%client, '', "\c32. \c6Place a bounty");
	// Warn
	if (!$RP::job::placeBounty[%client.RPData.value["jobID"]])
		messageClient(%client, '', "\c0Notice, that placing bounties is illegal.");
	%client.activateDatablock = %datablock;
	%client.activateState = 0;
	return true;
}

// Handle data
function BountyBrickData::parseData(%datablock, %client, %text)
{	
	%client.ThrowLastActive();
	
	%number = atoi(%text);
	switch (%client.activateState)
	{
		// Start
		case 0:
			switch (%number)
			{
				// Check bounties
				case 1:
					%client.stopActivate();
					%count = getWordCount($RP::bountyList);
					if (%count == 0)
					{
						messageClient(%client, '', "\c3There are no bounties.");
						return true;
					}
					messageClient(%client, '', "\c3Bounty List");
					for (%i = 0; %i < %count; %i++)
					{
						%key = getWord($RP::bountyList, %i);
						%bounty = RPDB.get(%key, "bounty");
						messageClient(%client, '', '\c3   %1 - %2', RPDB.get(%key, "name"), %bounty);
					}
				// Place bounty
				case 2:
					messageClient(%client, '', "\c6Insert the \c3name\c6 or \c3BL_ID\c6 of the person you want to place the bounty on.");
					%client.activateState = 2.1;
				// Invalid
				default:
					%client.stopActivate();
					return false;
			}
		// Place bounty
		case 2.1:
			%target = findClientByText(%text);
			if (!isObject(%target))
			{
				%client.stopActivate();
				messageClient(%client, '', '\c6Invalid target [\c3%1\c6].', %text);
				return true;
			}
			// Cannot bounty himself
			if (%target == %client)
			{
				%client.stopActivate();
				messageClient(%client, '', "\c6You cannot place a bounty on yourself.");
				return true;
			}
			%type = RPModExist("Money") ? $RP::pref::moneyName : "points";
			messageClient(%client, '', '\c6Insert the amount of \c3%1\c6 to be placed on targets head.', %type);
			
			%client.RPtarget = %target;
			%client.activateState = 2.2;
		// Place bounty
		case 2.2:
			%client.stopActivate();
			%target = %client.RPtarget;
			if (%number <= 0 || !isObject(%target))
				return false;
			
			if (RPModExist("Money"))
			{
				%number = (%number > %client.RPData.value["money"]) ? %client.RPData.value["money"] : %number;
				%amount = prepareDisplayMoney(getMetricPrefix(%number));
				%client.RPData.value["money"] -= %number;
			}
			else
			{
				%amount = %number @ "\c6 points";
			}
			
			if(%amount < 0)
			{
				messageClient(%client, '', "\c6You cannot place a negative amount of money on someone's head.");
				return false;
			}
			
			messageClient(%client, '', '\c6You placed \c3%1\c6 on \c3%2\c6\'s head.', %amount, %target.name);
			// Depending on amount, show name of the placer
			//if (%number >= $RP::pref::bounty::hidePlaceBounty)
				messageAll('', '\c3%1\c6 got \c3%2\c6 placed on his head.', %target.name, %amount);
			//else
			//	messageAll('', '\c3%1\c6 placed \c3%2\c6 on \c3%3\c6\'s head.', %client.name, %amount, %target.name);
			
			%target.RPData.value["bounty"] += %number;
			%client.RPtarget = "";
			AddToBountyList(%target.getSaveKey());
			
			// Cannot place bounties
			if (RPModExist("Jobs") && !$RP::job::placeBounty[%client.RPData.value["jobID"]])
				CommitCrime(%client, "Illegally placing a bounty", $RP::pref::crime::placeBounty);
		// Invalid
		default:
			%client.stopActivate();
			return false;
	}
	return true;
}

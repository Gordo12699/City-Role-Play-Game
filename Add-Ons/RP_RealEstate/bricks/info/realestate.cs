// ============================================================
// Project				:	CRPG
// Author				:	Iban
// Description			:	Real Estate Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Trigger Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CRPGREBrickData)
{
	brickfile = "Add-Ons/RP_RealEstate/TvOlive.blb";
	iconName = "Add-Ons/RP_Core/LcdTV";	
	
	category = "CRPG";
	subCategory = "Admin event bricks";
	
	uiName = "Real Estate Brick";
	
	specialBrickType = "";
	isInfoBrick = 1;
	adminOnly = 1;
};

// ============================================================
// Section 2 : Trigger Data
// ============================================================
function CRPGREBrickData::Activate(%datablock, %client)
{
	messageClient(%client, '', "\c3Realestate");
	messageClient(%client, '', "\c6Welcome! Please type the number corresponding to the options below.");
			
	messageClient(%client, '', "\c31. \c6View Lots");
	messageClient(%client, '', "\c32. \c6Buy Lot");
	messageClient(%client, '', "\c33. \c6Sell Lot");
	messageClient(%client, '', "\c34. \c6Cancel Sale");
			
	%client.stage = 0;
	%client.pickedLot = "";
	
	%client.activateDatablock = %datablock;
	%client.activateState = 0;
	return true;
}
function CRPGREBrickData::parseData(%datablock, %client, %text)
{
	if($RP::pref::server::debug)
	{
		talk(%datablock.uiName SPC %client.Name SPC %text);
	}
	
	%client.ThrowLastActive();
	%input = %text;
	switch (%client.activateState)
	{
		case 0:
			switch (%input)
			{
				//View Lots
				case 1:
					%client.stopActivate();
					%found = false;
					
					for(%a = 0; %a < ClientGroup.getCount(); %a++)
					{
						%subClient = ClientGroup.getObject(%a);
						if(getWord(%subClient.RPData.value["lotData"], 0) == 0)
						{
							if($RP::pref::server::debug)
							{
								talk('%subClient.RPData.value["lotData"] == 0, continuing');
							}
							
							continue;
						}
						
						%data = %subClient.RPData.value["lotData"];
						for(%b = 1; %b < getWordCount(%data); %b++)
						{
							if(%b % 2 == 0)
							{
								if($RP::pref::server::debug)
								{
									talk('%b % 2 == 0, continuing');
								}
								continue;
							}
							
							%newBrick = "_" @ getWord(%data, %b);
							
							if(isObject(%newBrick))
							{
								if($RP::pref::server::debug)
								{
									talk('Found lot for sale, setting lot in display');
								}
								%found = true;
								messageClient(%client, '', "\c3" @ getWord(%data, %b) @ "\c6, \c3" @ %newBrick.getDatablock().uiName SPC "\c6- Price: \c3$" @ getWord(%data, %b + 1) SPC "\c6Owner: \c3" @ ("brickGroup_" @ %subClient.bl_id).name);
							}
						}
					}
					
					if(!%found)
					{
						messageClient(%client, '', "\c6There are no lots to display.");
						return true;
					}
				//Buy Lot
				case 2:
					if(%client.brickGroup.lotsOwned < $CRPG::pref::realestate::maxLots)
					{
						messageClient(%client, '', "\c6Please enter the lot which you wish to buy.");
			
						%client.activateState = 2.1;
					}
					else
					{
						messageClient(%client, '', "\c6You already own enough lots.");
						%client.stage = 0;
						%client.stopActivate();
						return false;
					}
					
				//Sell Lot
				case 3:
					%nameLecture = true;
					%brick404 = true;
		
					%brickGroup = %client.brickGroup;
		
					for(%a = 0; %a < %brickGroup.getCount(); %a++)
					{
						if(%brickGroup.getObject(%a).getDatablock().CRPGBrickType == 1)
						{
							%brick = %brickGroup.getObject(%a);
				
							if(strReplace(strLwr(%client.RPData.value["lotData"]), strLwr(getSubStr(%brick.getName(), 1, strLen(%brick.getName()) - 1)), "") !$= strLwr(%client.RPData.value["lotData"]))
							{
								continue;
							}
				
							%brick404 = false;
			
							if(%brickGroup.getObject(%a).getName() !$= "")
							{
								%brick = %brickGroup.getObject(%a);
					
								%nameLecture = false;
					
								messageClient(%client, '', "\c6" @ getSubStr(%brick.getName(), 1, strLen(%brick.getName()) - 1) SPC "\c6- \c3" @ %brick.getDatablock().uiName);
							}
						}
					}
		
					if(%brick404)
					{
						messageClient(%client, '', "\c6There are no lots to display.");
			
						%client.stopActivate();
						return false;
					}
		
					if(%nameLecture)
					{
						messageClient(%client, '', "\c6You must give a name to one of your lots!");
			
						%client.stopActivate();
						return false;
					}
		
					messageClient(%client, '', "\c6Please enter the name of the lot you wish to sell.");
		
					%client.activateState = 3.1;
				
				//Cancel Sale
				case 4:
					%data = %client.RPData.value["lotData"];
		
					%found = false;
		
					for(%a = 1; %a < getWordCount(%data); %a++)
					{
						if(%a % 2 == 0)
						{
							continue;
						}
			
						%found = true;
			
						if(isObject("_" @ getWord(%data, %a)))
						{
							%brick = "_" @ getWord(%data, %a);
				
							messageClient(%client, '', "\c6" @ getWord(%data, %a) SPC "\c6- \c3" @ %brick.getDatablock().uiName);
						}
						else
						{
							messageClient(%client, '', "\c6" @ getWord(%data, %a) SPC "\c6- \c3Unknown Lot Type");
						}
					}
		
					if(!%found)
					{
						messageClient(%client, '', "\c6There are no lots to display.");
						%client.stopActivate();
						return false;
					}
		
					messageClient(%client, '', "\c6Please enter the name of the lot you wish to withdraw from sales.");
		
					%client.stage = 4.1;
					
				// Invalid
				default:
					%client.stopActivate();
					return false;
			}
		case 2.1:	
			%found = false;
		
			for(%a = 0; %a < ClientGroup.getCount(); %a++)
			{
				%subClient = ClientGroup.getObject(%a);
				
				if(getWord(%subClient.RPData.value["lotData"], 0) == 0 || %subClient.ID == %client.bl_id)
				{
					continue;
				}
			
				%data = %subClient.RPData.value["lotData"];
			
				for(%b = 1; %b < getWordCount(%data); %b++)
				{
					if(%b % 2 == 0)
						continue;
				
					if(strLwr(getWord(%data, %b)) $= %input)
					{
						%found = true;
						%newBrick = "_" @ getWord(%data, %b);
						%newBrickID = %newBrick.getID();
						%price = getWord(%data, %b + 1);
						%owner = getBrickGroupFromObject(%newBrickID);
					}
				}
			}
		
			if(!%found)
			{
				messageClient(%client, '', "\c6Invalid lot '\c3" @ %text @ "\c6'.");
				%client.stopActivate();
				return true;
			}
		
			if(%client.RPData.value["money"] < %price)
			{
				messageClient(%client, '', "\c6You need at least \c3$" @ %price SPC "\c6to buy this lot.");
				%client.stopActivate();
				return true;
			}
		
			messageClient(%client, '', "\c6You have bought \c3" @ getSubStr(%newBrick, 1, strLen(%newBrick) - 1) SPC "\c6from \c3" @ %owner.name @ "\c6.");
		
			if(isObject(%owner.client))
			{
				messageClient(%owner.client, '', "\c3" @ %client.name SPC "\c6has bought \c3" @ getSubStr(%newBrick, 1, strLen(%newBrick) - 1) SPC "\c6from you.");
			}
		
			%client.RPData.value["money"] -= %price;
			%owner.client.RPData.value["money"] += %price;
		
			%newBrickID.transferLot(%client.brickGroup);
			%newBrickID.setName("");
		
			%client.brickGroup.lotsOwned++;
			%owner.lotsOwned--;
		
			%client.brickGroup.taxes += %newBrickID.getDatablock().taxAmount;
			%owner.taxes -= %newBrickID.getDatablock().taxAmount;
		
			%client.displayInfo();
			%owner.client.displayInfo();
		
			for(%a = 0; %a < %client.brickGroup.getCount(); %a++)
			{
				%foundBrick = %client.brickGroup.getObject(%a);
				%foundBrick.tran = NULL;
			}
		
			%client.stopActivate();
			return true;
		
		case 3.1:
			if(%input $= "")
			{
				return true;
			}
		
			%picked = false;
		
			%brickGroup = %client.brickGroup;
		
			for(%a = 0; %a < %brickGroup.getCount(); %a++)
			{
				if(%brickGroup.getObject(%a).getDatablock().CRPGBrickType == 1)
				{
					if(%brickGroup.getObject(%a).getName() $= "")
					{
						continue;
					}
				
					%brick = %brickGroup.getObject(%a);
						
					if(strLwr(getSubStr(%brick.getName(), 1, strLen(%brick.getName()) - 1)) $= %input)
					{
						if(strReplace(strLwr(%client.RPData.value["lotData"]), strLwr(getSubStr(%brick.getName(), 1, strLen(%brick.getName()) - 1)), "") !$= strLwr(%client.RPData.value["lotData"]))
						{
							continue;
						}
					
						%picked = true;
					
						%client.pickedBrick = %brickGroup.getObject(%a);
					}
				}
			}
		
			if(!%picked)
			{
				messageClient(%client, '', "\c6Invalid lot '\c3" @ %text @ "\c6'.");
			
				return true;
			}
		
			%client.activateState = 3.2;
		
			messageClient(%client, '', "\c6Please enter the amount you wish to sell the lot for.");
		
			return true;
		
		case 3.2:
			%price = mFloor(%input);
		
			if(%price < 1)
			{
				messageClient(%client, '', "\c6Please enter a valid price!");
				return true;
			}
		
			messageClient(%client, '', "\c6Your offer has been put up.");
		
			if(getWord(%client.RPData.value["lotData"], 0) > 0)
			{
				%rest = getWords(%client.RPData.value["lotData"], 1, getWordCount(%client.RPData.value["lotData"]) - 1) @ " ";
			}
		
			%client.RPData.value["lotData"] = getWord(%client.RPData.value["lotData"], 0) + 1 SPC %rest @ getSubStr(%client.pickedBrick.getName(), 1, strLen(%client.pickedBrick.getName()) - 1) SPC %price;
		
			%client.stopActivate();
			return true;
		
		case 4.1:
			if(%input $= "")
			{
				return true;
			}
		
			%picked = false;
		
			%data = %client.RPData.value["lotData"];
		
			%newData = getWord(%data, 0) - 1;
		
			for(%a = 1; %a < getWordCount(%data); %a++)
			{
				if(%cancelNext)
				{
					%cancelNext = false;
				
						continue;
				}
			
				%newData = %newData SPC getWord(%data, %a);
			
				if(%a % 2 == 0)
				{
					continue;
				}
			
				if(strLwr(getWord(%data, %a)) $= %input)
				{
					%picked = true;
				
					%newData = getWords(%newData, 0, getWordCount(%newData) - 2);
			
					%cancelNext = true;
				}
			}
			
			if(!%picked)
			{
				messageClient(%client, '', "\c6Invalid lot '\c3" @ %text @ "\c6'.");
				return true;
			}
		
			messageClient(%client, '', "\c6Your offer has been withdrawn.");
		
			%client.RPData.value["lotData"] = %newData;
		
			%client.stopActivate();
			return true;
		//Invalid
		default:
			messageClient(%client, '', "\c3" @ %text SPC "\c6is not a valid option!");
			%client.stopActivate();
			return false;
	}
	return true;
}

package CRPG_RealEsate
{
	function fxDtsBrick::setName(%brick, %name)
	{
		%oldName = %brick.getName();
		
		if(isObject(%name) && (%brick.getDatablock().CRPGBrickType == 1 || %name.getDatablock().CRPGBrickType == 1))
		{
			%a = 1;
			
			while(isObject(%name @ %a))
			{
				%a++;
			}
			
			%name = %name @ %a;
		}
		
		parent::setName(%brick, %name);
		
		if(%name !$= "")
		{				
			for(%a = 1; %a <= ClientGroup.getCount(); %a++)
			{
				%subClient = ClientGroup.getObject(%a);
				
				if(getWord(%subClient.RPData.value["lotData"], 0) == 0)
				{
					continue;
				}
				
				%data = %subClient.RPData.value["lotData"];
				
				for(%b = 1; %b < getWordCount(%data); %b++)
				{
					if(%b % 2 == 0)
					{
						continue;
					}
					
					if(strLwr(getWord(%data, %b)) $= strLwr(getSubStr(%name, 1, strLen(%name) - 1)))
					{
						parent::setName(%brick, "");
						
						return;
					}
				}
			}
		}
		
		if(%oldName !$= "")
		{
			if(%brick.getName() $= "")
			{
				%brickGroup = getBrickGroupFromObject(%brick);
				%target = %brickGroup.client;
				%OwnerID = (!isObject(%target)) ? ($Server::serverType $= "Internet" ? %brickGroup.bl_id : %brickGroup.name) : %target.getSaveKey();
				RPDB.get(%OwnerID, "safe");
			
				%data = RPDB.get(%OwnerID, "lotData");
				
				%newData = getWord(%data, 0) - 1;
				
				for(%a = 1; %a < getWordCount(%data); %a++)
				{
					if(%cancelNext)
					{
						%cancelNext = false;
						
						continue;
					}
					
					%newData = %newData SPC getWord(%data, %a);
					
					if(%a % 2 == 0)
					{
						continue;
					}
					
					%newBrick = "_" @ getWord(%data, %a);
					
					if(isObject(%newBrick) && %newBrick $= %oldName)
					{
						%newData = getWords(%newData, 0, getWordCount(%newData) - 2);
						
						%cancelNext = true;
					}
				}
				
				RPDB.set(%OwnerID, "lotData", %newData);
			}
			else
			{
				RPDB.set(%OwnerID, "lotData", strReplace(RPDB.get(%OwnerID, "lotData"), getSubStr(%oldName, 1, strLen(%oldName) - 1), getSubStr(%name, 1, strLen(%name) - 1)));
			}
		}
	}
};
activatePackage(CRPG_RealEsate);

// ============================================================
// Project				:	CityRPG
// Author				:	Iban & Jookia
// Description			:	Criminal Bank Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Trigger Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CRPGDrugsellBrickData)
{
	brickfile = "./TvDrug.blb";
	iconName = "Add-Ons/RP_Core/LcdTV";	
	
	category = "CRPG";
	subCategory = "Admin event bricks";
	
	uiName = "Drug Sell Brick";
	
	specialBrickType = "";
	CRPGBrickType = 2;
	CRPGBrickAdmin = true;
	
	triggerDatablock = CRPGInputTriggerData;
	triggerSize = "2 4 1";
	trigger = 0;
	
	isInfoBrick = 1;
	adminOnly = 1;
};

// ============================================================
// Section 2 : Trigger Data
// ============================================================
function CRPGDrugsellBrickData::Activate(%datablock, %client)
{
	messageClient(%client, '', "\c3Drugs (New? /drughelp)");
	
	if (%client.RPData.value["totaldrugs"] > 0)
		messageClient(%client, '', '\c6You have \c3%1\c6 grams of \c3Opium \c6and \c3%2 \c6grams of \c3Marijuana\c6.', %client.RPData.value["opium"], %client.RPData.value["Marijuana"]);
	
	messageClient(%client, '', "\c31. \c6Sell Marijuana.");
	messageClient(%client, '', "\c32. \c6Sell Opium.");
	
	%client.activateDatablock = %datablock;
	%client.activateState = 0;
	return true;
}

function CRPGDrugsellBrickData::parseData(%datablock, %client, %text)
{
	%client.ThrowLastActive();
	%number = atoi(%text);
	switch(%client.activateState)
	{
		//Start
		case 0:
			switch (%number)
			{
				//Sell Marijuana
				case 1:
					%client.stage = 1.1;
					
					if(%client.RPData.value["Marijuana"] >= 1)
					{
						messageClient(%client,'',"\c6You have started selling.");
						%client.drugname = "marijuana";
						%client.selling = true;
						startSelling(%client);
						%client.startSelling();
					}
					else
					{
						messageClient(%client,'',"\c3You don't have any marijuana to sell.");
						%client.drugname = "";
						%client.selling = false;
						%client.stopActivate();
					}
				//Sell Opium
				case 2:
					%client.stage = 1.2;
			
					if(%client.RPData.value["Opium"] >= 1)
					{
						messageClient(%client,'',"\c6You have started selling.");
						%client.drugname = "opium";
						%client.selling = true;
						startSelling(%client);
					}
					else
					{
						messageClient(%client,'',"\c3You don't have any opium to sell.");
						%client.drugname = "";
						%client.selling = false;
						%client.stopActivate();
					}
				//Invalid
				default:
					%client.stopActivate();
					return 0;
			}
		//Invalid
		default:
			%client.stopActivate();
			return 0;
	}
	return 1;
}

	//if(%triggerStatus !$= "")
	//{
	//	if(%triggerStatus == true && %client.stage $= "")
	//	{
	//		messageClient(%client, '', "\c3What do you want to sell? \c6(New? /drughelp)");
	//		messageClient(%client, '', "\c31 \c6- Sell Marijuana.");
	//		messageClient(%client, '', "\c32 \c6- Sell Opium.");
			
	//		%client.drugname = "";
	//		%client.selling = false;
						
	//		%client.stage = 0;
	//	}
		
	//	if(%triggerStatus == false && %client.stage !$= "")
	//	{
	//		%client.selling = false;
	//		messageClient(%client, '', "\c6You are no longer selling.");
	//		%client.drugname = "";
	//		%client.stage = "";
	//	}
		
	//	return;
	//}
	
	//%input = strLwr(%text);
	
	//if(mFloor(%client.stage) == 0)
	//{
	//	if(strReplace(%input, "1", "") !$= %input || strReplace(%input, "one", "") !$= %input)
	//	{
	//		%client.stage = 1.1;
			
	//		if(CityRPGData.getData(%client.bl_id).valueMarijuana >= 1)
	//		{
	//			messageClient(%client,'',"\c6You have started selling.");
	//			%client.drugname = "marijuana";
	//			%client.selling = true;
	//		}
	//		else
	//		{
	//			messageClient(%client,'',"\c3You don't have any marijuana to sell.");
	//			%client.drugname = "";
	//			%client.selling = false;
	//		}
	//	}
	//}

	//if(mFloor(%client.stage) == 0)
	//{
	//	if(strReplace(%input, "2", "") !$= %input || strReplace(%input, "two", "") !$= %input)
	//	{
	//		%client.stage = 1.2;
			
	//		if(CityRPGData.getData(%client.bl_id).valueopium >= 1)
	//		{
	//			messageClient(%client,'',"\c6You have started selling.");
	//			%client.drugname = "opium";
	//			%client.selling = true;
	//		}
	//		else
	//		{
	//			messageClient(%client,'',"\c3You don't have any opium to sell.");
	//			%client.drugname = "";
	//			%client.selling = false;
	//		}
	//	}
	//}
//}

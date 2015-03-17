// ============================================================
// Project				:	CRPG
// Author				:	Iban & Jookia
// Description			:	Code used by all walks of life.
// ============================================================
// Table of Contents
// 1. Game CRPG Functions
// 2. Brick CRPG Functions
// 3. Client CRPG Functions
// 4. Trigger Bullshit
// 5. Client->Server Commands
// 6. Local Misc. Functions
// 7. Misc. Shit Functions
// ============================================================
if (isPackage(RP_Drugs_Game_Boot))
	deactivatePackage(RP_Drugs_Game_Boot);

package RP_Drugs_Game_Boot
{
	// Booting game
	function BootRP()
	{
		if ($RP::activated)
			return;
		Parent::BootRP();
		
		RPDB.addValue("Marijuana", 0);
		RPDB.addValue("Opium", 0);
		RPDB.addValue("totaldrugs", 0);
		RPDB.saveDefs();
	}
};
activatePackage(RP_Drugs_Game_Boot);

function Drug_Tick(%client)
{
	//echo("Initiating Drug Tick");
	
	CRPGData.lastDrugTickOn = $Sim::DTime;
	
	CRPG_DoDrugLoop(0);
	
	if(CRPGData.scheduleDrug)
		cancel(CRPGData.scheduleDrug);

	CRPGData.scheduleDrug = schedule(((getRandom($CRPG::drug::minSellSpeed,$CRPG::drug::maxSellSpeed)) * 1000), false, "Drug_Tick");
	
	startSelling(%client);
}

function CRPG_DoDrugLoop(%loop2,%client)
{
	%drugtime = (((getRandom($CRPG::drug::minSellSpeed,$CRPG::drug::maxSellSpeed)) * 1000) / CRPGData.dataCount);
	
	if(isObject(%client = findClientByBL_ID(CRPGData.data[%loop2].ID)))
		%client.drugtick();
		
	if(%client.selling)
	{
		startSelling(%client);
	}
	
	if(%loop2 < CRPGData.dataCount)
		schedule(%drugtime, false, "CRPG_DoDrugLoop", (%loop2 + 1));
		//echo("isselling main = " @ CRPGData.getData(%client.bl_id).valueIsSelling);
}

Drug_Tick();

// ============================================================
// Section 2 : Brick CRPG Functions
// ============================================================
//Drugs
function servercmddrugamount(%client)
{
	messageClient(%client,'',%client.RPData.value["drugamount"]);
}

function fxDTSBrick::bagPlant(%col)
{
	%col.schedule(0, "delete");
	
	getBrickGroupFromObject(%col).client.RPData.value["drugamount"]--;
			
	if(isObject(getBrickGroupFromObject(%col).client))
	{
		getBrickGroupFromObject(%col).client.SetInfo();
	}
}

function fxDTSBrick::startGrowing(%drug,%brick)
{
	%drug.isGrowing = true;
	%drug.canchange = false;
	%drug.currentColor = 45;
	
	if(%drug.uiName $= "marijuana")
	{
		%drugtype = $CRPG::drugs::marijuana::growthTime;
		%drugtime = ((($CRPG::drugs::marijuana::growthTime) * 60000) / 8);
	}
	
	else if(%drug.uiName $= "opium")
	{
		%drugtype = $CRPG::drugs::marijuana::growthTime;
		%drugtime = ((($CRPG::drugs::opium::growthTime) * 60000) / 8);
	}
	
	//color progression
	if(%drug.growtime == 1)
	{
		%drug.canchange = true;
		%drug.currentColor = 54;
		%drug.setColor(54);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 2)
	{
		%drug.canchange = true;
		%drug.currentColor = 55;
		%drug.setColor(55);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 3)
	{
		%drug.canchange = true;
		%drug.currentColor = 56;
		%drug.setColor(56);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 4)
	{
		%drug.canchange = true;
		%drug.currentColor = 57;
		%drug.setColor(57);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 5)
	{
		%drug.canchange = true;
		%drug.currentColor = 58;
		%drug.setColor(58);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 6)
	{
		%drug.canchange = true;
		%drug.currentColor = 59;
		%drug.setColor(59);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 7)
	{
		%drug.canchange = true;
		%drug.currentColor = 60;
		%drug.setColor(60);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 8)
	{
		%drug.canchange = true;
		%drug.currentColor = 61;
		%drug.setColor(61);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	%drug.canbecolored = false;
	
	if(%drug.growtime < 8)
	{
		%drug.hasemitter = false;
		%drug.canchange = false;
		%drug.growtime++;
		%drug.schedule(%drugtime, "startGrowing", %drug,%brick);
	}
	else if(%drug.growtime == 8)
	{
		%drug.canchange = true;
		%drug.grow();
		%drug.canchange = false;
	}
}

function fxDTSBrick::grow(%drug,%brick)
{
	%drug.health = 0;
	%drug.hasDrug = true;
	%drug.grew = true;
	%drug.setColor(61);
	%drug.canChange = true;
	%drug.cansetemitter = true;
	%drug.emitter = "GrassEmitter";
	%drug.setEmitter(GrassEmitter);
	%drug.cansetemitter = false;
	%drug.hasemitter = true;
	%drug.canchange = false;
}

function fxDTSBrick::harvest(%this, %client)
{	
	%drug = %this.getID();
	%brickData = %this.getDatablock();
	if(%this.hasDrug)
	{
		if(%drug.health < %drug.random)
		{
			%drug.health++;
			%percentage = mFloor((%drug.health / %drug.random) * 100);
			
			// cool color effect
			if(%percentage >= 0 && %percentage < 10)
				%color = "<color:ff0000>";
			else if(%percentage >= 10 && %percentage < 20)
				%color = "<color:ff2200>";
			else if(%percentage >= 10 && %percentage < 30)
				%color = "<color:ff4400>";
			else if(%percentage >= 10 && %percentage < 40)
				%color = "<color:ff6600>";
			else if(%percentage >= 10 && %percentage < 50)
				%color = "<color:ff8800>";
			else if(%percentage >= 10 && %percentage < 60)
				%color = "<color:ffff00>";
			else if(%percentage >= 10 && %percentage < 70)
				%color = "<color:88ff00>";
			else if(%percentage >= 10 && %percentage < 80)
				%color = "<color:66ff00>";
			else if(%percentage >= 10 && %percentage < 90)
				%color = "<color:44ff00>";
			else if(%percentage >= 10 && %percentage < 100)
				%color = "<color:22ff00>";
			else if(%percentage == 100)
				%color = "<color:00ff00>";
				
			commandToClient(%client,'centerPrint',"\c3" @ %brickData.uiName @ " \c6harvested: %" @ %color @ "" @ %percentage,3);
		}
		else
		{
			if(%brickData.drugType $= "Opium" && %brickData.drugType !$= "Marijuana")
			{
				%harvestamt = getRandom($CRPG::drugs::opium::harvestMin,$CRPG::drugs::opium::harvestMax);
				%client.RPData.value["Opium"] += %harvestamt;
				if(%client.RPData.value["Opium"] > 50)
				{
					messageClient(%client, '', "\c6You cannot carry more than \c350 \c6grams of \c3Opium\c6.");
					%client.RPData.value["Opium"] = 50;
				}
				
				%client.RPData.value["totaldrugs"] += %client.RPData.value["Opium"] + %client.RPData.value["Marijuana"];
				if(%client.bl_id == %drug.owner)
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of \c3Opium\c6.",3);
				}
				else
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of someone else's \c3Opium\c6.",3);
				}
				
				%drug.canchange = true;
				%drug.currentColor = 45;
				%drug.setColor(45);
				%drug.grew = false;
				%drug.health = 0;
				%drug.growtime = 0;
				%drug.canbecolored = false;
				%drug.watered = false;
				%drug.random = getRandom($CRPG::drugs::opium::harvestMin,$CRPG::drugs::opium::harvestMax);
				%drug.hasDrug = false;
				%drug.isGrowing = false;
				%drug.cansetemitter = true;
				%drug.setEmitter(None);
				%drug.cansetemitter = false;
				%drug.canchange = false;

				%client.SetInfo();
			}
			else if(%brickData.drugType $= "Marijuana" && %brickData.drugType !$= "Opium")
			{
				%harvestamt = getRandom($CRPG::drugs::marijuana::harvestMin,$CRPG::drugs::marijuana::harvestMax);
				%client.RPData.value["marijuana"] += %harvestamt;
				if(%client.RPData.value["Marijuana"] > 50)
				{
					messageClient(%client, '', "\c6You cannot carry more than \c350 \c6grams of \c3Marijuana\c6.");
					%client.RPData.value["Marijuana"] = 50;
				}
				
				%client.RPData.value["totaldrugs"] += %client.RPData.value["Opium"] + %client.RPData.value["Marijuana"];
				if(%client.bl_id == %drug.owner)
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of \c3Marijuana\c6.",3);
				}
				else
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of someone else's \c3Marijuana\c6.",3);
				}
				
				%drug.canchange = true;
				%drug.currentColor = 45;
				%drug.setColor(45);
				%drug.grew = false;
				%drug.health = 0;
				%drug.growtime = 0;
				%drug.canbecolored = false;
				%drug.watered = false;
				%drug.random = getRandom($CRPG::drugs::marijuana::harvestMin,$CRPG::drugs::marijuana::harvestMax);
				%drug.hasDrug = false;
				%drug.isGrowing = false;
				%drug.cansetemitter = true;
				%drug.setEmitter(None);
				%drug.cansetemitter = false;
				%drug.canchange = false;

				%client.SetInfo();

			}
		}
	}
	//else
	//commandToClient(%client,'centerPrint',"\c6This plant isn't ready to harvest yet!",1);
}

function startSelling(%client)
{
	%drugname = %client.drugname;
	if(%drugname $= "marijuana")
	{
		%amount = %client.RPData.value["Marijuana"];
	}
	else if(%drugname $= "opium")
	{
		%amount = %client.RPData.value["Opium"];
	}
	
	if(%amount > 0)
	{
		%buymin = $CRPG::drug::minBuyAmt;
		%buymax = $CRPG::drug::maxBuyAmt;
		%grams = getRandom(%buymin,%buymax);
		
		if(%grams > %amount)
		{
			%grams = %amount;
		}
		else if(%grams == 0)
		{
			messageClient(%client,'',"\c6You're all out!");
			return;
		}
		
		%grams = mFloor(%grams);
		
		if(%drugname $= "marijuana")
		{
			%profit = $CRPG::drugs::marijuana::basePrice;
		}
		
		else if(%drugname $= "opium")
		{
			%profit = $CRPG::drugs::opium::basePrice;
		}
		
		%totalcash = %grams * %profit;
		
		%randomize = getRandom(1,2);
		if(%randomize == 1) 
		{
			%totalcash -= getRandom(0.75,1);
		}
		else if(%randomize == 2)
		{
		%totalcash += getRandom(1,1.25);
		}
		%totalcash = mFloor(%totalcash);
		
		%client.RPData.value["Money"] += %totalcash;
		//%client.setGameBottomPrint();
		
		%slang = %grams;
		switch(%slang)
		{
			case 1:
				%slang = "a \c3gram\c6 of";
				//return;
			case 2:
				%slang = "a \c3dimebag\c6 of";
				//return;
			case 3:
				%slang = "\c3three grams\c6 of";
				//return;
			case 4:
				%slang = "a \c3dub\c6 of";
				//return;
			case 5:
				%slang = "\c3five grams\c6 of";
				//return;
			default:
				%slang = "some";
		}
		%client.updateInfo();
		messageClient(%client,'',"\c6You sold " @ %slang @ " " @ %drugname @ " to a stranger for \c3$" @ %totalcash @"\c6.");
		
		if(%drugname $= "marijuana")
		{
			%client.RPData.value["Marijuana"] -= %grams;
			%client.RPData.value["totaldrugs"] -= %grams;
		}
		
		else if(%drugname $= "opium")
		{
			%client.RPData.value["Opium"] -= %grams;
			%client.RPData.value["totaldrugs"] -= %grams;
		}
		
		if(%grams >= 10)
		{
			return;
		}
	}
	else
	{
		messageClient(%client,'',"\c6You're all out!");
		return;
	}
		
	startselling(%client);
}

function servercmdmydrugs(%client)
{
	messageClient(%client,'',"\c6Your marijuana in grams :" @ %client.RPData.value["Marijuana"]);
	messageClient(%client,'',"\c6Your opium in grams: " @ %client.RPData.value["opium"]);
	messageClient(%client,'',"\c6Your total drugs in grams: " @ %client.RPData.value["totaldrugs"]);
}

function servercmddrughelp(%client)
{
messageClient(%client,'',"\c6- \c3How to grow drugs for dummies\c6 -");
	messageClient(%client,'',"\c3Step 1\c6: Navigate to the CRPG tab in the brick menu");
	messageClient(%client,'',"\c3Step 2\c6: Scroll down until you find the drug bricks");
	messageClient(%client,'',"\c3Step 3\c6: Select a drug and place it in your CRPG Lot");
	messageClient(%client,'',"\c3Step 4\c6: Click your drug brick to water it");
	messageClient(%client,'',"\c3Step 5\c6: Wait a few in-game days");
	messageClient(%client,'',"\c3Step 6\c6: Find/buy a knife");
	messageClient(%client,'',"\c3Step 7\c6: Harvest your drug brick with your knife");
	messageClient(%client,'',"\c3Step 8\c6: Find a drug sell brick placed around the city");
	messageClient(%client,'',"\c3Step 9\c6: Don't get caught!");
	messageClient(%client,'',"\c6---");
	messageClient(%client,'',"\c3Tip\c6: Having a lot of drugs on you and getting hit with a baton will get you jail time!");
	messageClient(%client,'',"\c3Tip\c6: Cops can baton your crops and turn them in as evidence, so hide them well!");
}
//Drugs end

// more drugs
function gameConnection::drugtick(%client)
{
	if(%client.selling)
	{
		startSelling(%client);
	}
	%client.SetInfo();
}

datablock ParticleData(DrugsmokeParticle)
{
   dragCoefficient      = 1.0;
   gravityCoefficient   = -0.2;
   inheritedVelFactor   = 0.0;
   constantAcceleration = 1.0;
   lifetimeMS           = 8000;
   lifetimeVarianceMS   = 300;
   useInvAlpha          = true;
   textureName          = "Add-Ons/RP_Drugs/shapes/cloud";
   colors[0]     = "1.0 1.0 1.0 1.0";
   colors[1]     = "1.0 1.0 1.0 1.0";
   colors[2]     = "1.0 1.0 1.0 0.0";
   sizes[0]      = 1.5;
   sizes[1]      = 1.5;
   sizes[2]      = 1.5;
   times[0]      = 0.0;
   times[1]      = 0.2;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(DrugsmokeEmitter)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 0;
   ejectionVelocity = 0.2;
   ejectionOffset   = 1.5;
   velocityVariance = 0.49;
   thetaMin         = 0;
   thetaMax         = 30;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "DrugsmokeParticle";

   uiName = "Drugsmoke";
};
datablock ShapeBaseImageData(DrugsmokeImage)
{
   shapeFile = "base/data/shapes/empty.dts";
	emap = false;

	mountPoint = $HeadSlot;

	stateName[0]					= "Ready";
	stateTransitionOnTimeout[0]		= "FireA";
	stateTimeoutValue[0]			= 0.06;

	stateName[1]					= "FireA";
	stateTransitionOnTimeout[1]		= "Done";
	stateWaitForTimeout[1]			= True;
	stateTimeoutValue[1]			= 0.9;
	stateEmitter[1]					= DrugsmokeEmitter;
	stateEmitterTime[1]				= 0.9;

	stateName[2]					= "Done";
	stateScript[2]					= "onDone";
};

datablock AudioProfile(Smokingsound)
{
   filename    = "Add-Ons/Gamemode_CRPG/sounds/smokedaweed.wav";
   description = AudioClose3d;
   preload = true;
};

function servercmdusemarijuana(%client)
{
    %time = getSimTime();
    if(%client.lastTimeusemarijuana $= "" || %time - %client.lastTimeusemarijuana > 5000)
	{
		if(isObject(%client.player))
		{
			if(%client.RPData.value["Marijuana"] > 0)
			{
				serverplay3d(Smokingsound,%client.player.getHackPosition() SPC "0 0 1 0");
			
				echo(%client.name @ " smoke");
				%client.player.setWhiteout(1);
				%client.player.emote(DrugsmokeImage);
				messageClient(%client,'',"\c6Used \c31 gram \c6of \c3marijuana\c6.");
				%client.RPData.value["Marijuana"]--;
				%client.lastTimeusemarijuana = %time;
			}
			else
			messageClient(%client,'',"\c6You don't have any.");
		}	
		else
		messageClient(%client,'',"\c6You must spawn first.");
	}
}

function DrugsmokeImage::onDone(%this,%obj,%slot)
{
	%obj.unMountImage(%slot);
}
// more drugs

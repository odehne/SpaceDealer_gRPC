﻿CREATE TABLE "Journeys" ("Id" TEXT NOT NULL, "ShipId" TEXT NOT NULL, "CurrentDistance" REAL, "State" INTEGER, "DeparturePlanetId" TEXT,	"DestinationPlanetId"	TEXT,	"CurrentSectorX" INTEGER, "CurrentSectorY" INTEGER,	"CurrentSectorZ" INTEGER, "PiratShipId"	TEXT, "NewlyDiscoveredPlanetId"	TEXT, PRIMARY KEY("Id"))
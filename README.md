# _Band_

#### _List of bands and where they are playing_

#### By _eric nicolas_

## Description

_Input bands and venues and connect them using SQL tables. Webapp allows you to modify, modify update items. The webapp displayed wheere bands are gonna play in a certain or what bands are playing in a certain venue_

## Setup/Installation Requirements

* _web browser, MAMP, .Net, Mysqlconnector_


## Known Bugs

_Cant test the delete function because the id is not being reset_

## sql commands
_
create database band_tracker;
use band_tracker;
CREATE TABLE bands (id serial PRIMARY KEY, bandname VARCHAR(255));
CREATE TABLE venues (id serial PRIMARY KEY, venuename VARCHAR(255));
CREATE TABLE band_venues (id serial PRIMARY KEY, venue_id int, band_id int);


## Support and contact details

_eric.e.nicolas@gmail.com_

## Technologies Used

_C#, .Net, mysql_

### License

*MIT*

Copyright (c) 2016 **_eric nicolas_**
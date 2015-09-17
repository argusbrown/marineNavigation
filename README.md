Marine Navigation
=================

*NMEA (National Marine Electronice Association) 2000 - Marine Navigation and Instrumentation for NGT-1.*

This is for reading NMEA2k messages from an NMEA2k bus.  Included is a device driver for the Actisense NGT-1 (http://www.actisense.com/products/nmea-2000/ngt-1/ngt-1.html).  This codebase works well with, for example, the Maretron SSC 200 (http://www.maretron.com/products/ssc200.php) Solid State Compass for these bus messages:
- Heading
- Pitch
- Roll
- Rate of turn

The only dependency is Stateless (https://github.com/nblumhardt/stateless), which is used to maintain the progress of messages arriving in on the NMEA2k bus.  Same principles can be used with the electronically compatible CAN (https://en.wikipedia.org/wiki/CAN_bus) and SAE J1939 vehicle bus systems and other protocols (PLGR, etc.).

Usage of an abstracted SerialPort allows dependency injection to make it easy to test the variety of NMEA2k messages and even to replay a saved stream of data (ie: from files) for validation and performance testing of recorded sailing activities.  This strategy works well for other serial ports besides RS-232 (RS-485, RS-422, etc.).  

This project was published on Sept. 17, 2015 after being dormant for 3+ years.  If there is interest, I can publish a fork with visual instrumentation and additional message (device) support.

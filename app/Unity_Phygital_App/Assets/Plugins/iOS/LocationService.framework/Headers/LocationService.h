//
//  LocationService.h
//  LocationService
//
//  Created by Chandrakumar Sekar on 05/10/23.
//

#import <Foundation/Foundation.h>
#import <CoreLocation/CoreLocation.h>
#import <UserNotifications/UserNotifications.h>

//! Project version number for LocationService.
FOUNDATION_EXPORT double LocationServiceVersionNumber;

//! Project version string for LocationService.
FOUNDATION_EXPORT const unsigned char LocationServiceVersionString[];

// In this header, you should import all the public headers of your framework using statements like #import <LocationService/PublicHeader.h>


@interface LocationService : NSObject <CLLocationManagerDelegate>

@property (nonatomic, strong) CLLocationManager *locationManager;

+ (void) initializePlugin;
+ (void) startUpdatingLocation: (NSString *) arrayLocationString forParams: (NSString *) stringParams;
+ (void) stopUpdatingLocation;

@end



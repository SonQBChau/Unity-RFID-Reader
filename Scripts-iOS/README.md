# Import Project from zip file


Download GoogleService-Info and put it in Assets folder


If Unity failed to connect to Firebase, import Firebase again, then copy the FirebaseDatabase.dll in Assets/Firebase/Plugins and replace the one in Mono


# To build iOS app from Unity

1. In Unity, click build iOS
2. Open the folder in xcode, auto signing and build

3. If build failed, Product-> Clean Build Folder
4. Open Podfile by text editor and remove all Firebase version, ex: pod 'Firebase/Database'
5. Open terminal in build ios folder
6. Pod install (pod update if needed)
7. Open xcworkplace, not xcodeproj
8. Click Build

Build zip file to distribute to Testflight or Hockey app:
1. Click Product->Archive
2. Distribute App
3. Ad hoc, uncheck bitcode
4. Use Automatically manage signing

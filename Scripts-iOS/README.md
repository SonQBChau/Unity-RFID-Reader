
# To build iOS app from Unity

1. In Unity, click build iOS
2. Open the folder in xcode, auto signing and build

3. If build failed, Product-> Clean Build Folder
4. Open Podfile and remove Firebase version
5. Pod install (pod update if needed)
6. Open xcworkplace, not xcodeproj
7. Click Build

Build zip file to distribute to Testflight or Hockey app:
1. Click Product->Archive
2. Distribute App
3. Ad hoc, uncheck bitcode
4. Use Automatically manage signing

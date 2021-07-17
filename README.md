# About DDJDS-SFR
<pre>
Doodle Jump DS Save File Reader (otherwise known as DDJDS-SFR) is a simple CLI utility which its purpose is
to be able to read any Doodle Jump DS Save File.

Simplicity & Stability is the main goal of this application.
</pre>
## More Doodle Jump DS Content ##
<a href="https://github.com/miso-xyz/nds-notes/blob/main/AR%20Codes%20ive%20made/Doodle%20Jump%20Codes.txt">nds-notes - Doodle Jump DS - Action Replay Codes</a></br>
<a href="https://github.com/miso-xyz/nds-notes/tree/main/Doodle-Jump-DS">nds-notes - Doodle Jump DS Stuff</a>

# Arguments (v1.0.1)
On default settings, the only animation that will be playing will be the blinking end text.
Name | Effect | Default
------------- | ------------- | -------------
--noAnim | No animations will be shown | false
--allAnim | All animations will be shown | false

# To Do
- [x] Read Settings
- [ ] Rendering Errors can happen if lagging while processing multiple animations (v1.0.1)
- [ ] Signature Stuff
  - [ ] Figuring out `"Unknown Signature"`
  - [ ] Validating Signatures

# Changelog
<details>
<summary>v1.0.1</summary>
<pre>- Added Settings Reading
- Added Custom Arguments
- Changed HighScore Type from "UInt16" to "Int32"
- Fixed Drawing randomization
- GUI Improvements (Added Animations, Moved some stuff around & recolored text)
  |- Animations
     |- Blinking Pause Text
     |- Blinking "Max Reached!" & "(2.147B)"
- .DSV Reading is more stable
- Added Save File Type Style
  |- ".DSV - DeSmuME"
  |- ".SAV - Default"
  |- "%fileType% - Unknown"</pre>
</details>
<details>
  <summary>Screenshots</summary>
  <p>v1.0.1 Release Screenshot:</p>
  <img src="https://i.imgur.com/R5HLBln.png"></img>*
  
  - - - -
  
  <p>v1.0 Release Screenshot:</p>
  <img src="https://i.imgur.com/tNz8ayN.png"></img>
</details>

# Credits
<a href="https://www.deviantart.com/pingasthelawler/">pingasthelawler</a> - <a href="https://www.deviantart.com/pingasthelawler/art/Super-Sonic-Doodle-Jump-Alien-429184537">App Icon</a>

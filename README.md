<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->

[![LinkedIn][linkedin-shield]][linkedin-url]

<!-- PROJECT LOGO -->
<br />
<p align="center">
  <a href="https://github.com/Nickprovs/SoundsOfSpacetime.Mobile">
    <img src="_meta/logo.png" alt="Logo" width="256" height="256">
  </a>

  <h3 align="center">Sounds of Spacetime - Mobile</h3>

  <p align="center">
    Our purpose is to explore the physics of gravitational waves via an analogy to audible sounds. Gravitational waves (GW) are ripples in the fabric of spacetime produced by colliding black holes, neutron stars, supernovae, and other astrophysical phenomena. 
    <br />
    <br />
  </p>
</p>

<!-- TABLE OF CONTENTS -->

## Table of Contents

- [About the Project](#about-the-project)
  - [Built With](#built-with)
- [Getting Started](#getting-started)
  - [Installation](#installation)
- [License](#license)

<!-- ABOUT THE PROJECT -->

## About The Project

This app allows users to experience customized Gravitational Waves caused by coalescing binaries. It is also an informational tool to learn more about gravitational waves and the existing detections.
Sounds of Spacetime is built with Xamarin Forms. As such, it is a cross-platform mobile app. Any platform specific logic routes to native code through an interface.


[![Product Name Screen Shot][product-screenshot]](/_meta/sample_simulator_1_android.jpg)
[![Product Name Screen Shot][product-screenshot]](/_meta/sample_simulator_1_ios.jpg)
### Built With

- Xamarin Forms
- SciChart
- OxyPlot
- Microsoft Open Solving Library for Ordinary Differential Equations (ODEs)

<!-- GETTING STARTED -->

### Installation

1. Clone or Fork the project

```sh
git clone https://github.com/Nickprovs/SoundsOfSpacetime.Mobile.git
```
2. Install Dependencies via Nuget

3. Replace SciChart plotting implementation with OxyPlot implementation in AppDelegate.cs (iOS) and MainActivity (Android).

```sh
//From one of these (depending no platform)
containerRegistry.RegisterSingleton<IPlotService, SciChartService_iOS>();
containerRegistry.RegisterSingleton<IPlotService, SciChartService_Android>();

//To this
containerRegistry.RegisterSingleton<IPlotService, OxyPlotService>();
```

The reason for this is that I use SciChart in production because it's much faster than OxyPlot - but it's propietary and requires a key.
However, you can still pull and run the app if you swap out the plotting service.

<!-- LICENSE -->

## License

Distributed under the MIT License. See [License](LICENSE.md) for more information.

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->

[license-shield]: https://img.shields.io/badge/License-MIT-yellow.svg
[license-url]: https://github.com/nickprovs/ballpit/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=flat-square&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/nickprovs
[product-screenshot]: _meta/sample.jpg

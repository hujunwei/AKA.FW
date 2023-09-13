/**
=========================================================
* Material Dashboard 2 React - v2.1.0
=========================================================

* Product Page: https://www.creative-tim.com/product/material-dashboard-react
* Copyright 2022 Creative Tim (https://www.creative-tim.com)

Coded by www.creative-tim.com

 =========================================================

* The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
*/

import { useState, useEffect } from "react";

// @mui material components
import Card from "@mui/material/Card";
// import Grid from "@mui/material/Grid";
import { Divider } from "@mui/material";

// Material Dashboard 2 React components
import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";

// Material Dashboard 2 React base styles
import breakpoints from "assets/theme/base/breakpoints";

// Images
import redirectorImage from "assets/images/Redirector2.png";

function Header() {
  const [tabsOrientation, setTabsOrientation] = useState("horizontal");

  useEffect(() => {
    // A function that sets the orientation state of the tabs.
    function handleTabsOrientation() {
      return window.innerWidth < breakpoints.values.sm
        ? setTabsOrientation("vertical")
        : setTabsOrientation("horizontal");
    }

    /** 
     The event listener that's calling the handleTabsOrientation function when resizing the window.
    */
    window.addEventListener("resize", handleTabsOrientation);

    // Call the handleTabsOrientation function to set the state with the initial value.
    handleTabsOrientation();

    // Remove event listener on cleanup
    return () => window.removeEventListener("resize", handleTabsOrientation);
  }, [tabsOrientation]);

  const baseUrl = window.location.origin;
  const fullUrl = `"${baseUrl}/{your_alias}"`;

  return (
    <MDBox position="relative" mb={5}>
      <MDBox
        display="flex"
        alignItems="center"
        position="relative"
        minHeight="19.6rem"
        borderRadius="xl"
        sx={{
          backgroundImage: ({ functions: { rgba, linearGradient }, palette: { gradients } }) =>
            `${linearGradient(
              rgba(gradients.info.main, 0.1),
              rgba(gradients.info.state, 0.2)
            )}, url(${redirectorImage})`,
          backgroundSize: "cover",
          backgroundPosition: "50%",
          overflow: "hidden",
        }}
      />
      <Card
        sx={{
          position: "relative",
          mt: -8,
          mx: 3,
          py: 2,
          px: 2,
        }}
      >
        <MDTypography variant="h3" color="dark">
          Extreme alias redirecting experience with Redirector
        </MDTypography>
        <MDTypography variant="button" color="text" fontWeight="regular">
          Just 2 steps to follow
        </MDTypography>
        <Divider variant="middle" />
        <MDTypography variant="body2" fontWeight="regular">
          Although domain providers have ability to forward domain, they
          usually do not support deep linking if they do not support DNS alias record. Which means anything after your domain is not supported
          for redirecting. So here comes the Redirector. Without it, AKA.FREEWHEEL already works perfectly to redirect to custom aliased links
          by typing {fullUrl} in your browser address bar.
          But with Redirector, it requires you type even less boilderplate base address like &quot;{baseUrl}&quot;. And you can also configure any base url you like in your browser.
          <br />
          <br />
          Redirector is a light-weighted browser extension that helps you override url typed in
          browser. It enables you to directly type &quot;aka.fw/your_own_alias&quot; in your browser
          address bar and deep linking to {fullUrl}. And it supports most of modern browsers like
          Chrome, Edge, Firefox, etc.
          <br />
          <br />     
        </MDTypography>
        <MDTypography variant="body1" fontWeight="regular">
          Instructions:
          <br />
          <br />
        </MDTypography>
        <MDTypography variant="body2" fontWeight="regular">
          <b>Step1</b> Add the extension{" "}
          <a href="https://chrome.google.com/webstore/detail/redirector/ocgpenflpmgnfapjedencafcfakcekcd?hl=en">
            Redirector
          </a>
          .
          <br />
        </MDTypography>

        <MDTypography variant="body2" fontWeight="regular">
          <b>Step2</b> Import <a href="https://github.freewheel.tv/hackathon2022/HACK-380-AKA.FREEWHEEL/blob/master/Aka_Fw_Redirector_Config.json">Aka_Fw_Redirector_Config.json</a> in the
          extension.
          <br />
        </MDTypography>

        <MDTypography variant="body2" fontWeight="regular">
          <br />
          <b>NOTE:</b> You could also customize your own redirecting base address by editing the config file or in Redirector extension.
          <br />
        </MDTypography>
      </Card>
    </MDBox>
  );
}

export default Header;

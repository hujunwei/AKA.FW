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

// Authentication layout components
import BasicLayout from "layouts/authentication/components/BasicLayout";

// Images
import bgImage from "assets/images/freewheel_redirecting.png";

import useToken from "utilities/UseToken";
import Constants from "utilities/Constants";
import useErrorHandler from "utilities/useErrorHandler";

import { Card } from "@mui/material";
import CardActions from "@mui/material/CardActions";
import CardContent from "@mui/material/CardContent";
import Button from "@mui/material/Button";
import MDTypography from "components/MDTypography";
import { useNavigate } from "react-router-dom";

function Redirect() {
  const { token } = useToken();
  const [redirectError, setRedirectError] = useState(false);
  const { checkAndConvertResponse } = useErrorHandler();

  const alias = window.location.pathname.replace("/", "");
  const navigate = useNavigate();
  const redirectToMain = () => navigate("/dashboard");

  // TODO: how to solve fail to fetch error?
  async function findTargetUrlToRedirect() {
    return fetch(`${Constants.API_URL_REDIRECT}/${alias}`, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    })
      .then(checkAndConvertResponse)
      .then((responseJson) => {
        if (responseJson.isError) {
          setRedirectError(true);
          return;
        }

        setRedirectError(false);
        window.location.replace(responseJson.targetUrl);
      });
  }

  useEffect(async () => {
    await findTargetUrlToRedirect();
  }, []);

  return (
    <BasicLayout image={bgImage}>
      {redirectError && (
        <Card>
          <CardContent>
            <MDTypography variant="h4" color="error" fontWeight="medium" gutterBottom>
              AN ERROR OCCURRED
            </MDTypography>
            <MDTypography variant="caption" color="text">
              Please check your alias is correct. If issue persists, please{" "}
              <a href="mailto:jwhu@apac.freewheel.com">contact us</a>.
            </MDTypography>
          </CardContent>
          <CardActions>
            <Button size="small" onClick={redirectToMain}>
              Home
            </Button>
          </CardActions>
        </Card>
      )}
    </BasicLayout>
  );
}

export default Redirect;

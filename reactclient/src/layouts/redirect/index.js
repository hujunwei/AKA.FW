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

function Redirect() {
  const { token } = useToken();
  const [ redirectError, setRedirectError] = useState(false);
  const { renderAlert, checkAndConvertResponse } = useErrorHandler();

  const alias = window.location.pathname.replace("/", "");

  async function findTargetUrlToRedirect() {
    return fetch(`${Constants.API_URL_REDIRECT}/${alias}` , {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json"
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
      { redirectError && renderAlert() }
    </BasicLayout>
  );
}

export default Redirect;

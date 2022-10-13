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

import { useState } from "react";

// react-router-dom components
import { Link, useNavigate } from "react-router-dom";

// @mui material components
import Card from "@mui/material/Card";
import Switch from "@mui/material/Switch";
import LoadingButton from "@mui/lab/LoadingButton";

// Material Dashboard 2 React components
import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";
import MDInput from "components/MDInput";

// Authentication layout components
import BasicLayout from "layouts/authentication/components/BasicLayout";

// Images
import bgImage from "assets/images/background3.png";

import useToken from "utilities/UseToken";
import Constants from "utilities/Constants";
import useErrorHandler from "utilities/useErrorHandler";

function Basic() {
  const [rememberMe, setRememberMe] = useState(false);
  const [userName, setUserName] = useState();
  const [password, setPassword] = useState();
  const [loading, setLoading] = useState();
  const [loginError, setLoginError] = useState(false);
  const { setToken } = useToken();
  const { renderAlert, checkAndConvertResponse } = useErrorHandler();

  const navigate = useNavigate();

  async function loginUser(credentials) {
    return fetch(Constants.API_URL_LOGIN, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(credentials),
    })
      .then(checkAndConvertResponse)
      .then((data) => {
        if (!data.isError) {
          setLoginError(false);
          setLoading(false);
          setToken(data);
          navigate("/official");
        } else {
          setLoginError(true);
          setLoading(false);
        }
      });
  }

  const handleSetRememberMe = () => setRememberMe(!rememberMe);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoginError(false);
    setLoading(true);

    await loginUser({
      userName,
      password,
    });
  };

  return (
    <BasicLayout image={bgImage}>
      <Card>
        <MDBox
          variant="gradient"
          bgColor="info"
          borderRadius="lg"
          coloredShadow="info"
          mx={2}
          mt={-3}
          p={2}
          mb={1}
          textAlign="center"
          isfw={1}
        >
          <MDTypography variant="h4" fontWeight="medium" color="white" mt={1}>
            AKA.FREEWHEEL <br />
          </MDTypography>
          <MDTypography component={Link} to="/authentication/sign-up"variant="caption" fontWeight="small" color="white" mt={1}>
            We haven&apos;t support LDAP. If you do not have an account with us, please <b>sign up</b> first using your freewheel email account first .
          </MDTypography>
         
        </MDBox>
        <MDBox pt={4} pb={3} px={3}>
          <MDBox component="form" role="form" onSubmit={handleSubmit}>
            <MDBox mb={2}>
              <MDInput
                type="email"
                label="@freewheel.com/@apac.freewheel.com"
                onChange={(e) => setUserName(e.target.value)}
                fullWidth
              />
            </MDBox>
            <MDBox mb={2}>
              <MDInput
                type="password"
                label="Password"
                onChange={(e) => setPassword(e.target.value)}
                fullWidth
              />
            </MDBox>
            <MDBox display="flex" alignItems="center" ml={-1}>
              <Switch checked={rememberMe} onChange={handleSetRememberMe} />
              <MDTypography
                variant="button"
                fontWeight="regular"
                color="text"
                onClick={handleSetRememberMe}
                sx={{ cursor: "pointer", userSelect: "none", ml: -1 }}
              >
                &nbsp;&nbsp;Remember me
              </MDTypography>
            </MDBox>
            <MDBox mt={4} mb={1}>
              <LoadingButton
                color="success"
                type="submit"
                loading={loading}
                variant="contained"
                fullWidth
                sx={{ 
                  color: '#fff',
                  backgroundColor: '#390a8b',
                  '&:hover': {
                    backgroundColor:'#390a8b',
                    opacity: 0.8
                  }}}
              >
                sign in
              </LoadingButton>
            </MDBox>
            {loginError && renderAlert()}
            <MDBox mt={3} mb={1} textAlign="center">
              <MDTypography variant="button" color="text">
                Don&apos;t have an account?{" "}
                <MDTypography
                  component={Link}
                  to="/authentication/sign-up"
                  variant="button"
                  color="info"
                  fontWeight="medium"
                  textGradient
                >
                  Sign up
                </MDTypography>
              </MDTypography>
            </MDBox>
          </MDBox>
        </MDBox>
      </Card>
    </BasicLayout>
  );
}

export default Basic;

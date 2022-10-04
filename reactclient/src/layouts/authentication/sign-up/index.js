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
import LoadingButton from "@mui/lab/LoadingButton";

// Material Dashboard 2 React components
import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";
import MDInput from "components/MDInput";

// Authentication layout components
import BasicLayout from "layouts/authentication/components/BasicLayout";

// Images
import bgImage from "assets/images/background3.png";

import Constants from "utilities/Constants";
import useErrorHandler from "utilities/useErrorHandler";

function Cover() {
  const [username, setUserName] = useState();
  const [password, setPassword] = useState();
  const [nickname, setNickName] = useState();
  const [loading, setLoading] = useState();
  const [registerError, setRegisterError] = useState();
  const { renderAlert, checkAndConvertResponse } = useErrorHandler();
  
  const navigate = useNavigate();

  async function rigisterUser(credentials) {
    return fetch(Constants.API_URL_REGISTER, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(credentials),
    })
      .then(checkAndConvertResponse)
      .then((data) => {
        if (!data.isError) {
          setRegisterError(false);
          setLoading(false);
          navigate("/authentication/sign-in");
        } else {
          setRegisterError(true);
          setLoading(false);
        }
      });
  }

  const handleSubmit = async (e) => {
    e.preventDefault();
    setRegisterError(false);
    setLoading(true);

    // We enforce username to be email
    const email = username;

    await rigisterUser({
      nickname,
      username,
      password,
      email,
    });
  };

  return (
    <BasicLayout image={bgImage}>
      <Card>
        <MDBox
          variant="gradient"
          bgColor="info"
          borderRadius="lg"
          coloredShadow="success"
          mx={2}
          mt={-3}
          p={3}
          mb={1}
          textAlign="center"
          isfw={1}
        >
          <MDTypography variant="h6" fontWeight="medium" color="white" mt={1}>
            Start AKA.FREEWHEEL today
          </MDTypography>
          <MDTypography display="block" variant="button" color="white" my={1}>
            Enter your nickname, username and password to register.
          </MDTypography>
        </MDBox>

        <MDBox pt={4} pb={3} px={3}>
          <MDBox component="form" role="form" onSubmit={handleSubmit}>
            <MDBox mb={2}>
              <MDBox mb={2}>
                <MDInput
                  type="text"
                  label="Nickname"
                  onChange={(e) => setNickName(e.target.value)}
                  fullWidth
                />
              </MDBox>
              <MDInput
                type="email"
                label="Username"
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
            <MDBox mt={4} mb={1}>
              <LoadingButton
                color="success"
                type="submit"
                loading={loading}
                variant="contained"
                style={{color: 'white', backgroundColor: '#390a8b'}}
                fullWidth
              >
                register
              </LoadingButton>
            </MDBox>
            {registerError && renderAlert()}
            <MDBox mt={3} mb={1} textAlign="center">
              <MDTypography variant="button" color="text">
                Already have an account?{" "}
                <MDTypography
                  component={Link}
                  to="/authentication/sign-in"
                  variant="button"
                  color="info"
                  fontWeight="medium"
                  textGradient
                >
                  Sign In
                </MDTypography>
              </MDTypography>
            </MDBox>
          </MDBox>
        </MDBox>
      </Card>
    </BasicLayout>
  );
}

export default Cover;

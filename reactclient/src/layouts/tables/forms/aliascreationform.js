import React, { useState } from "react";

import { Card } from "@mui/material";
import { LoadingButton } from "@mui/lab";

import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";
import MDInput from "components/MDInput";

import Constants from "utilities/Constants";
import useErrorHandler from "utilities/useErrorHandler";

import useToken from "utilities/UseToken";

export default function AliasCreationForm(props) {
  /* eslint-disable react/prop-types */
  const { onAliasCreated } = props;

  const [ name, setName ] = useState(null);
  const [ alias, setAlias ] = useState(null);
  const [ targetUrl, setTargetUrl ] = useState(null);
  const { renderAlert, checkAndConvertResponse } = useErrorHandler();
  const [ addError, setAddError ] = useState(false);
  const [ loading, setLoading ] = useState(false);
  const { token } = useToken();

  const handleSubmit = (e) => {
    e.preventDefault();

    setLoading(true);

    const mappingToCreate = {
        name,
        sourceAlias: alias,
        targetUrl,
        isActive: true,
        isOfficial: false,
      };

    fetch(Constants.API_URL_CREARE_MAPPING, {
      method: "POST",
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
      body: JSON.stringify(mappingToCreate),
    })
      .then(checkAndConvertResponse)
      .then((responseJson) => {
        if (responseJson.isError) {
          setAddError(true);
          setLoading(false);
          return;
        }

        setAddError(false);
        setLoading(false);
         /* eslint-disable react/prop-types */
        onAliasCreated(mappingToCreate);
      });
  };

  return (
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
      >
        <MDTypography variant="h4" fontWeight="medium" color="white" mt={1}>
          Add a new alias
        </MDTypography>
      </MDBox>
      <MDBox pt={4} pb={3} px={3}>
        <MDBox component="form" role="form" onSubmit={handleSubmit}>
          <MDBox mb={2}>
            <MDInput
              type="text"
              label="Name"
              onChange={(e) => setName(e.target.value)}
              placeholder="Friendly name..."
              fullWidth
            />
          </MDBox>
          <MDBox mb={2}>
            <MDInput
              type="text"
              label="Alias"
              onChange={(e) => setAlias(e.target.value)}
              placeholder="Custom alias name, case insensitive..."
              fullWidth
            />
          </MDBox>
          <MDBox mb={2}>
            <MDInput
              type="text"
              label="Target URL"
              onChange={(e) => setTargetUrl(e.target.value)}
              placeholder="Full url, e.g 'https://www.google.com'..."
              fullWidth
            />
          </MDBox>
          <MDBox mt={4} mb={1}>
            <LoadingButton
              color="info"
              type="submit"
              loading={loading}
              loadingPosition="start"
              variant="contained"
            >
              Add
            </LoadingButton>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <LoadingButton
              color="success"
              loading={false}
              loadingPosition="start"
              variant="contained"
              onClick={() => onAliasCreated(null)}
            >
              Cancel
            </LoadingButton>
          </MDBox>
          {addError && renderAlert()}
        </MDBox>
      </MDBox>
    </Card>
  );
}

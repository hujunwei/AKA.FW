import React, { useState } from "react";

import { Card } from "@mui/material";
import { LoadingButton } from "@mui/lab";

import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";
import MDInput from "components/MDInput";

import Constants from "utilities/Constants";
import useErrorHandler from "utilities/useErrorHandler";

import useToken from "utilities/UseToken";

export default function AliasUpdateForm(props) {
  /* eslint-disable react/prop-types */
  const { editingUrl, onAliasUpdated } = props;

  const [ name, setName ] = useState(editingUrl.name);
  const [ alias, setAlias ] = useState(editingUrl.sourceAlias);
  const [ targetUrl, setTargetUrl ] = useState(editingUrl.targetUrl);

  const { renderAlert, checkAndConvertResponse } = useErrorHandler();
  const [ updateError, setUpdateError ] = useState(false);
  const [ loading, setLoading ] = useState(false);
  const { token } = useToken();

  const handleSubmit = (e) => {
    e.preventDefault();

    setLoading(true);

    const mappingToUpdate = {
        name,
        sourceAlias: alias,
        targetUrl,
        isActive: true,
        isOfficial: false,
      };

    fetch(`${Constants.API_URL_UPDATE_MAPPING}/${editingUrl.id}`, {
      method: "PATCH",
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
      body: JSON.stringify(mappingToUpdate),
    })
      .then(checkAndConvertResponse)
      .then((responseJson) => {
        if (responseJson.isError) {
          setUpdateError(true);
          setLoading(false);
          return;
        }

        setUpdateError(false);
        setLoading(false);
         /* eslint-disable react/prop-types */
        onAliasUpdated(mappingToUpdate);
      });
  };

  return (
    <Card>
      <MDBox
        variant="gradient"
        bgColor="primary"
        borderRadius="lg"
        coloredShadow="primary"
        mx={2}
        mt={-3}
        p={2}
        mb={1}
        textAlign="center"
      >
        <MDTypography variant="h4" fontWeight="medium" color="white" mt={1}>
          Editing {name}
        </MDTypography>
      </MDBox>
      <MDBox pt={4} pb={3} px={3}>
        <MDBox component="form" role="form" onSubmit={handleSubmit}>
          <MDBox mb={2}>
            <MDInput
              type="text"
              label="Name"
              value={name}
              onChange={(e) => setName(e.target.value)}
              placeholder="Friendly name..."
              fullWidth
            />
          </MDBox>
          <MDBox mb={2}>
            <MDInput
              type="text"
              label="Alias"
              value={alias}
              onChange={(e) => setAlias(e.target.value)}
              placeholder="Custom alias name, case insensitive..."
              fullWidth
            />
          </MDBox>
          <MDBox mb={2}>
            <MDInput
              type="text"
              label="Target URL"
              value={targetUrl}
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
              Update
            </LoadingButton>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <LoadingButton
              color="success"
              loading={false}
              loadingPosition="start"
              variant="contained"
              onClick={() => onAliasUpdated(null)}
            >
              Cancel
            </LoadingButton>
          </MDBox>
          {updateError && renderAlert()}
        </MDBox>
      </MDBox>
    </Card>
  );
}

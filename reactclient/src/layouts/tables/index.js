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

import { useEffect, useState } from "react";

// @mui material components
import Grid from "@mui/material/Grid";
import Card from "@mui/material/Card";
import AddIcon from "@mui/icons-material/Add";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import { IconButton, Divider, Tooltip } from "@mui/material";
import Backdrop from "@mui/material/Backdrop";
import Modal from "@mui/material/Modal";
import Fade from "@mui/material/Fade";

// Material Dashboard 2 React components
import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";
import MDBadge from "components/MDBadge";
import MDButton from "components/MDButton";
import MDSpinner from "react-md-spinner";

// Material Dashboard 2 React example components
import DashboardLayout from "examples/LayoutContainers/DashboardLayout";
import DashboardNavbar from "examples/Navbars/DashboardNavbar";
import Footer from "examples/Footer";
import DataTable from "examples/Tables/DataTable";
import { Navigate } from "react-router-dom";

import Constants from "utilities/Constants";
import useToken from "utilities/UseToken";
import useErrorHandler from "utilities/useErrorHandler";
import stringTruncate from "utilities/stringTruncate";

import AliasCreationForm from "./forms/aliascreationform";
import AliasUpdateForm from "./forms/aliasupdateform";

const formatDate = (dateString) => {
  const options = { year: "numeric", month: "long", day: "numeric" };
  return new Date(dateString).toLocaleDateString(undefined, options);
};

const style = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
};

function Tables() {
  const columns = [
    { Header: "name", accessor: "name", width: "20%", align: "left" },
    { Header: "alias", accessor: "alias", width: "10%", align: "left" },
    { Header: "target url", accessor: "targeturl", width: "40%", align: "left" },
    { Header: "updated on", accessor: "updatedat", width: "10%", align: "left" },
    { Header: "status", accessor: "isactive", width: "10%", align: "center" },
    { Header: "actions", accessor: "actions", width: "10%", align: "center" },
  ];
  const { token } = useToken();
  const [loading, setLoading] = useState(false);
  const [loadUserUrlsError, setLoadUserUrlsError] = useState(false);
  const { renderAlert, checkAndConvertResponse, checkResponse } = useErrorHandler();
  const [tableData, setTableData] = useState([]);
  const [openAdd, setOpenAdd] = useState(false);
  const handleOpenAdd = () => setOpenAdd(true);
  const handleCloseAdd = () => setOpenAdd(false);
  const [openEdit, setOpenEdit] = useState(false);
  const [editingUrl, setEditingUrl] = useState(null);
  const handleOpenEdit = (url) => {
    setEditingUrl(url);
    setOpenEdit(true);
  };
  const handleCloseEdit = () => setOpenEdit(false);

  async function loadUserUrls() {
    setLoadUserUrlsError(false);

    return fetch(Constants.API_URL_LIST_USER_MAPPINGS, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    })
      .then(checkAndConvertResponse)
      .then((responseJson) => {
        setLoading(false);

        if (responseJson.isError) {
          setLoadUserUrlsError(true);
          return;
        }

        setLoadUserUrlsError(false);
        setTableData(responseJson);
      });
  }

  const onAliasCreated = async (createdAlias) => {
    if (createdAlias) {
      await loadUserUrls();
    }

    setOpenAdd(false);
  };

  const onAliasUpdated = async (updatedAlias) => {
    if (updatedAlias) {
      await loadUserUrls();
    }

    setOpenEdit(false);
  };

  const onAliasDeleted = (deletedAliasId) => {
    const tableDataCopy = [...tableData];

    const index = tableDataCopy.findIndex((row) => row.id === deletedAliasId);

    if (index !== -1) {
      tableDataCopy.splice(index, 1);
    }

    setTableData(tableDataCopy);
  };

  async function deleteUrl(url) {
    setLoading(true);

    fetch(`${Constants.API_URL_DELETE_MAPPING}/${url.id}`, {
      method: "DELETE",
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })
      .then(checkResponse)
      .then((responseJson) => {
        setLoading(false);

        if (responseJson.isError) {
          alert(`Error deleting link alias mapping with name: ${url.name}`);
          return;
        }

        onAliasDeleted(url.id);
      });
  }

  useEffect(async () => {
    setLoading(true);
    await loadUserUrls();
  }, []);

  const rows = tableData.map((url) => ({
    name: (
      <Tooltip title={url.name} disableInteractive>
        <MDTypography variant="caption" color="text" fontWeight="medium">
          {stringTruncate(url.name, 25)}
        </MDTypography>
      </Tooltip>
    ),
    alias: (
      <Tooltip title={url.sourceAlias} disableInteractive>
        <MDTypography variant="caption" color="text" fontWeight="medium">
          {stringTruncate(url.sourceAlias, 20)}
        </MDTypography>
      </Tooltip>
    ),
    targeturl: (
      <Tooltip title={url.targetUrl} disableInteractive>
        <MDTypography variant="caption" color="text" fontWeight="medium">
          {stringTruncate(url.targetUrl, 50)}
        </MDTypography>
      </Tooltip>
    ),
    updatedat: (
      <MDTypography variant="caption" color="text" fontWeight="medium">
        {formatDate(url.updatedAt)}
      </MDTypography>
    ),
    isactive: (
      <MDTypography variant="caption" color="text" fontWeight="medium">
        {url.isActive ? (
          <MDBadge color="success" badgeContent="Active" container />
        ) : (
          <MDBadge color="error" badgeContent="Deactivated" container />
        )}
      </MDTypography>
    ),
    actions: (
      <MDBox
        sx={{
          display: "flex",
          alignItems: "center",
        }}
      >
        <IconButton aria-label="edit" size="small" onClick={() => handleOpenEdit(url)}>
          <EditIcon color="info" />
        </IconButton>
        <Divider
          orientation="vertical"
          sx={{
            height: "20px",
          }}
        />
        <IconButton
          aria-label="delete"
          size="small"
          onClick={async () => {
            if (
              window.confirm(`Are you sure you want to delete the alias link named "${url.name}"?`)
            )
              await deleteUrl(url);
          }}
        >
          <DeleteIcon color="error" />
        </IconButton>
      </MDBox>
    ),
  }));

  return !token ? (
    <Navigate replace to="/authentication/sign-in" />
  ) : (
    <DashboardLayout>
      <DashboardNavbar />
      <MDBox pt={6} pb={3}>
        <Grid container spacing={6}>
          <Grid item xs={12}>
            <Card>
              <MDBox
                mx={2}
                mt={-3}
                py={3}
                px={2}
                variant="gradient"
                bgColor="warning"
                borderRadius="lg"
                coloredShadow="dark"
              >
                <MDBox display="flex" justifyContent="space-between" alignItems="center" p={1}>
                  <MDBox>
                    <MDTypography variant="h6" color="white">
                      Create or Manage your own alias
                    </MDTypography>
                  </MDBox>
                  <MDBox>
                    {loading && <MDSpinner aria-label="Loading..." />}
                    {!loading && !loadUserUrlsError && (
                      <MDButton variant="gradient" color="success" onClick={handleOpenAdd}>
                        <AddIcon>Add</AddIcon>
                      </MDButton>
                    )}
                    {loadUserUrlsError && renderAlert()}
                  </MDBox>
                </MDBox>
              </MDBox>
              <MDBox pt={3}>
                <DataTable
                  table={{ columns, rows }}
                  isSorted={false}
                  entriesPerPage={false}
                  showTotalEntries={false}
                  noEndBorder
                />
              </MDBox>
            </Card>
          </Grid>
        </Grid>
      </MDBox>
      <Footer />
      <Modal
        aria-labelledby="transition-modal-title"
        aria-describedby="transition-modal-description"
        open={openAdd}
        onClose={handleCloseAdd}
        closeAfterTransition
        BackdropComponent={Backdrop}
        BackdropProps={{
          timeout: 500,
        }}
      >
        <Fade in={openAdd}>
          <MDBox sx={style}>
            <MDTypography id="transition-modal-description" sx={{ mt: 2 }}>
              <AliasCreationForm onAliasCreated={onAliasCreated} />
            </MDTypography>
          </MDBox>
        </Fade>
      </Modal>
      <Modal
        aria-labelledby="transition-modal-title"
        aria-describedby="transition-modal-description"
        open={openEdit}
        onClose={handleCloseEdit}
        closeAfterTransition
        BackdropComponent={Backdrop}
        BackdropProps={{
          timeout: 500,
        }}
      >
        <Fade in={openEdit}>
          <MDBox sx={style}>
            <MDTypography id="transition-modal-description" sx={{ mt: 2 }}>
              <AliasUpdateForm editingUrl={editingUrl} onAliasUpdated={onAliasUpdated} />
            </MDTypography>
          </MDBox>
        </Fade>
      </Modal>
    </DashboardLayout>
  );
}

export default Tables;

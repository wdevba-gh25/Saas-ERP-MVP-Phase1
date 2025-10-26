// src/App.tsx
import { useEffect } from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import SignUpPage from "./pages/SignUpPage";
import ProjectsPage from "./pages/ProjectsPage";
import ProtectedRoute from "./components/ProtectedRoute";
import LoadingSpinner from "./components/LoadingSpinner";
import { useAuthStore } from "./store/auth.store";

function App() {
  const { authLoading, finishLoading } = useAuthStore();

  useEffect(() => {
    // simulate small delay so spinner shows nicely
    const timer = setTimeout(() => finishLoading(), 200);
    return () => clearTimeout(timer);
  }, [finishLoading]);

  if (authLoading) {
    return <LoadingSpinner />;
  }

  return (
    <Routes>
      {/* Redirect root path to login */}
      <Route path="/" element={<Navigate to="/login" replace />} />

      {/* Public routes */}
      <Route path="/login" element={<LoginPage />} />
      <Route path="/signup" element={<SignUpPage />} />

      {/* Protected routes */}
      <Route
        path="/projects"
        element={
          <ProtectedRoute>
            <ProjectsPage />
          </ProtectedRoute>
        }
      />
    </Routes>
  );
}

export default App;
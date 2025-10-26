// src/pages/ProjectsPage.tsx
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuthStore } from "../store/auth.store";

export default function ProjectsPage() {
  const { token, logout } = useAuthStore();
  const navigate = useNavigate();

  useEffect(() => {
    if (!token) {
      navigate("/login");
    }
  }, [token, navigate]);

  return (
    <div className="p-6">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Projects</h1>
        <button
          onClick={logout}
          className="btn-secondary"
        >
          Logout
        </button>
      </div>

      <p className="text-gray-700">
        This is a placeholder for your projects page.
      </p>
    </div>
  );
}
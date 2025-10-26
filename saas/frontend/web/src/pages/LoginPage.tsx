import { useEffect } from "react";
import { useNavigate, Link } from "react-router-dom";
import LoginForm from "../components/LoginForm";
import { useAuthStore } from "../store/auth.store";

export default function LoginPage() {
  const navigate = useNavigate();
  const { token } = useAuthStore();

  useEffect(() => {
    if (token) {
      navigate("/projects");
    }
  }, [token, navigate]);

  return (
    <div className="mx-auto max-w-xl p-6">
      <h1 className="text-2xl font-semibold mb-2">Welcome back</h1>
      <p className="text-sm text-gray-600 mb-6">
        New here?{" "}
        <Link className="text-blue-600 hover:underline" to="/signup">
          Create an account
        </Link>
      </p>
      <div className="bg-white rounded-2xl shadow p-6">
        <LoginForm onLoggedIn={() => navigate("/projects")} />
      </div>
    </div>
  );
}
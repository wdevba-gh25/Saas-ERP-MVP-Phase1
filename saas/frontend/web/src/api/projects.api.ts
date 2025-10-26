import { client } from "./client";

export interface Project {
  id: string;
  name: string;
  description?: string;
}

export async function getProjects(): Promise<Project[]> {
  const { data } = await client.get<Project[]>("/api/projects");
  return data;
}
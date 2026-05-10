import axios from "axios";
import type { CreateSkillRequest, Skill } from "../../models/Skill";
import type { ISkillAPIService } from "./ISkillAPIService";

const API_URL = "/api/Skills";

export const skillApi: ISkillAPIService = {
    async getAllSkills(): Promise<Skill[]> {
        try {
            const res = await axios.get<Skill[]>(API_URL);
            return res.data || [];
        } catch (error) {
            console.error("Error fetching skills:", error);
            return [];
        }
    },

    async getSkillById(id: string): Promise<Skill | null> {
        try {
            const res = await axios.get<Skill>(`${API_URL}/${id}`);
            return res.data;
        } catch (error) {
            console.error(`Error fetching skill ${id}:`, error);
            return null;
        }
    },

    async getSkillByName(name: string): Promise<Skill | null> {
        try {
            const res = await axios.get<Skill>(`${API_URL}/byName/${name}`);
            return res.data;
        } catch (error) {
            console.error(`Error fetching skill by name ${name}:`, error);
            return null;
        }
    },
    
    async createSkill(skill: CreateSkillRequest): Promise<string> {
        try {
            const res = await axios.post<{ id: string }>(API_URL, { skill });
            return res.data.id;
        } catch (error) {
            console.error("Error creating skill:", error);
            throw error; 
        }
    },

    async deleteSkill(id: string): Promise<boolean> {
        try {
            const res = await axios.delete(`${API_URL}/${id}`);
            return res.status === 200;
        } catch (error) {
            console.error("Error deleting skill:", error);
            return false;
        }
    }
};
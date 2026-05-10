import axios from "axios";
import type { Candidate, CreateUpdateCandidateRequest } from "../../models/Candidate";
import type { ICandidateAPIService } from "./ICandidateAPIService";

const API_URL = "/api/Candidates";

export const candidateApi: ICandidateAPIService = {
    async getAllCandidates(): Promise<Candidate[]> {
        try {
            const res = await axios.get<Candidate[]>(API_URL);
            return res.data || [];
        } catch (error) {
            console.error("Error fetching all candidates:", error);
            return [];
        }
    },

    async getCandidateById(id: string): Promise<Candidate | null> {
        try {
            const res = await axios.get<Candidate>(`${API_URL}/${id}`);
            return res.data;
        } catch (error) {
            console.error(`Error fetching candidate ${id}:`, error);
            return null;
        }
    },

    async getCandidateByName(name: string): Promise<Candidate[]> {
        try {
            const res = await axios.get<Candidate[]>(`${API_URL}/byName/${name}`);
            return res.data || [];
        } catch (error) {
            console.error(`Error fetching candidates by name ${name}:`, error);
            return [];
        }
    },

    async createCandidate(candidate: CreateUpdateCandidateRequest): Promise<string> {
        try {
            const res = await axios.post<{ id: string }>(API_URL, candidate);
            return res.data.id;
        } catch (error) {
            console.error("Error creating candidate:", error);
            throw error;
        }
    },

    async updateCandidate(id: string, candidate: CreateUpdateCandidateRequest): Promise<boolean> {
        try {
            const res = await axios.put(`${API_URL}/${id}`, candidate);
            return res.status === 200;
        } catch (error) {
            console.error(`Error updating candidate ${id}:`, error);
            return false;
        }
    },

    async deleteCandidate(id: string): Promise<boolean> {
        try {
            const res = await axios.delete(`${API_URL}/${id}`);
            return res.status === 200;
        } catch (error) {
            console.error("Error deleting candidate:", error);
            return false;
        }
    },

    async addSkillToCandidate(candidateId: string, skillId: string): Promise<boolean> {
        try {
            const res = await axios.post(`${API_URL}/${candidateId}/skills/${skillId}`);
            return res.status === 200;
        } catch (error) {
            console.error("Error adding skill to candidate:", error);
            return false;
        }
    },

    async removeSkillFromCandidate(candidateId: string, skillId: string): Promise<boolean> {
        try {
            const res = await axios.delete(`${API_URL}/${candidateId}/skills/${skillId}`);
            return res.status === 200;
        } catch (error) {
            console.error("Error removing skill from candidate:", error);
            return false;
        }
    },

    async searchCandidates(name?: string, skillIds?: string): Promise<Candidate[]> {
        try {
            const res = await axios.get<Candidate[]>(`${API_URL}/search`, {
                params: { 
                    name: name || undefined, 
                    skillIds: skillIds || undefined 
                }
            });
            return res.data || [];
        } catch (error) {
            console.error("Error searching candidates:", error);
            return [];
        }
    }
};
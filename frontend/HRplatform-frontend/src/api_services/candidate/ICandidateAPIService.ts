import type { Candidate, CreateUpdateCandidateRequest } from "../../models/Candidate";

export interface ICandidateAPIService {
    getAllCandidates(): Promise<Candidate[]>;
    getCandidateById(id: string): Promise<Candidate | null>;
    getCandidateByName(name: string): Promise<Candidate[]>;

    createCandidate(candidate: CreateUpdateCandidateRequest): Promise<string>;
    updateCandidate(id: string, candidate: CreateUpdateCandidateRequest): Promise<boolean>; 
    deleteCandidate(id: string): Promise<boolean>;

    addSkillToCandidate(candidateId: string, skillId: string): Promise<boolean>;
    removeSkillFromCandidate(candidateId: string, skillId: string): Promise<boolean>;

    searchCandidates(name?: string, skillIds?: string): Promise<Candidate[]>;
}
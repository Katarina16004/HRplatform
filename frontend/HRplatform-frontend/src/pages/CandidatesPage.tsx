import { useEffect, useState } from 'react';
import { candidateApi } from '../api_services/candidate/CandidateAPIService';
import type { Candidate, CreateUpdateCandidateRequest } from '../models/Candidate';
import CandidateTable from '../components/candidate/CandidateTable';
import CandidateSearch from '../components/candidate/CandidateSearch';
import CandidateForm from '../components/candidate/CandidateAddUpdateForm';
import toast from 'react-hot-toast';

const CandidatesPage = () => {
    const [candidates, setCandidates] = useState<Candidate[]>([]);
    const [loading, setLoading] = useState(true);
    const [editingCandidate, setEditingCandidate] = useState<Candidate | null>(null);

    const refresh = async () => {
        setLoading(true);
        try {
            const data = await candidateApi.getAllCandidates();
            setCandidates(data);
        } catch (err) {
            console.error("Error:", err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => { refresh(); }, []);

    const handleCreateOrUpdate = async (data: CreateUpdateCandidateRequest): Promise<boolean> => {
        try {
            if (editingCandidate) {
                const success = await candidateApi.updateCandidate(editingCandidate.id, data);
                if (success) {
                    toast.success("Candidate updated successfully!");
                    setEditingCandidate(null);
                } else {
                    toast.error("Failed to update candidate. Email might already be in use.");
                    return false;
                }
            } else {
                await candidateApi.createCandidate(data);
                toast.success("Candidate created successfully!");
            }
            await refresh();
            return true; 
        } catch (err: any) {
            const errorMessage = err.response?.data || "Operation failed";
            toast.error(errorMessage);
            return false;
        }
    };

    const handleDelete = async (id: string) => {
        if (window.confirm("Are you sure you want to delete this candidate?")) {
            try {
                await candidateApi.deleteCandidate(id);
                toast.success("Candidate deleted!");
                refresh();
            } catch (err: any) {
                toast.error(err.response?.data || "Error deleting");
            }
        }
    };
    const handleAddSkill = async (candidateId: string, skillId: string) => {
        try {
            const success = await candidateApi.addSkillToCandidate(candidateId, skillId);
            if (success) {
                toast.success("Skill added!");
                refresh();
            } else {
                toast.error("Failed to add skill.");
            }
        } catch (err) {
            toast.error("Error adding skill");
        }
    };

    const handleRemoveSkill = async (candidateId: string, skillId: string) => {
        try {
            const success = await candidateApi.removeSkillFromCandidate(candidateId, skillId);
            if (success) {
                toast.success("Skill removed");
                refresh();
            } else {
                toast.error("Failed to remove skill.");
            }
        } catch (err) {
            toast.error("Error removing skill");
        }
    };

    return (
        <div style={{ 
            maxWidth: '90%', 
            margin: '40px auto', 
            padding: '20px', 
            backgroundColor: 'white', 
            borderRadius: '8px', 
            boxShadow: '0 2px 10px #7998fb3d' 
        }}>
            <h1 style={{ paddingBottom: '10px' }}>Candidates</h1>
            
            <div style={{ 
                display: 'flex', 
                gap: '40px', 
                justifyContent: 'space-between', 
                alignItems: 'flex-start',
                marginBottom: '30px' 
            }}>
                <div style={{ flex: 1, border: '1px solid #f0f0f0', borderRadius: '8px', padding: '10px', background: '#b0d2ff' }}>
                    <CandidateForm 
                        onSubmit={handleCreateOrUpdate} 
                        initialData={editingCandidate} 
                    />
                    {editingCandidate && (
                        <button 
                            onClick={() => setEditingCandidate(null)}
                            style={{ 
                                padding: '6px 12px',
                                cursor: 'pointer',
                                fontSize: '15px',
                                border: '1px solid #ccc',
                                borderRadius: '4px',
                                background: 'white',
                                color: '#333'
                            }}
                        >
                            Cancel Edit
                        </button>
                    )}
                </div>

                <div style={{ flex: 1 }}>
                    <CandidateSearch onResultsFound={results => setCandidates(results)} onClear={refresh} />
                </div>
            </div>

            <hr style={{ border: '0', borderTop: '1px solid #eee', marginBottom: '30px' }} />

            {loading ? (
                <p>Loading data...</p>
            ) : (
                <CandidateTable 
                    candidates={candidates} 
                    onDelete={handleDelete} 
                    onEdit={(c) => { 
                        setEditingCandidate(c); 
                        window.scrollTo({ top: 0, behavior: 'smooth' }); 
                    }}
                    onAddSkill={handleAddSkill}
                    onRemoveSkill={handleRemoveSkill}
                />
            )}
        </div>
    );
};

export default CandidatesPage;